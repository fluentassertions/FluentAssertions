﻿#region

using System;
using System.Linq;
using FluentAssertions.Common;

#endregion

namespace FluentAssertions.Execution
{
    /// <summary>
    /// Represents an implicit or explicit scope within which multiple assertions can be collected.
    /// </summary>
    public class AssertionScope : IAssertionScope
    {
        #region Private Definitions

        private readonly IAssertionStrategy assertionStrategy;
        private readonly ContextDataItems contextData = new ContextDataItems();

        private Func<string> reason;
        private bool useLineBreaks;

        [ThreadStatic]
        private static AssertionScope current;

        private AssertionScope parent;
        private Func<string> expectation;
        private string fallbackIdentifier = "object";
        private bool? succeeded;

        #endregion

        private AssertionScope(IAssertionStrategy _assertionStrategy)
        {
            assertionStrategy = _assertionStrategy;
            parent = null;
        }

        /// <summary>
        /// Starts an unnamed scope within which multiple assertions can be executed
        /// and which will not throw until the scope is disposed.
        /// </summary>
        public AssertionScope()
            : this(new CollectingAssertionStrategy())
        {
            parent = current;
            current = this;

            if (parent != null)
            {
                contextData.Add(parent.contextData);
                Context = parent.Context;
            }
        }

        /// <summary>
        /// Starts a named scope within which multiple assertions can be executed and which will not throw until the scope is disposed.
        /// </summary>
        public AssertionScope(string context)
            : this()
        {
            Context = context;
        }

        /// <summary>
        /// Gets or sets the context of the current assertion scope, e.g. the path of the object graph
        /// that is being asserted on.
        /// </summary>
        public string Context { get; set; }

        /// <summary>
        /// Gets the current thread-specific assertion scope.
        /// </summary>
        public static AssertionScope Current
        {
            get => current ?? new AssertionScope(new DefaultAssertionStrategy());
            private set => current = value;
        }

        public IAssertionScope UsingLineBreaks
        {
            get
            {
                useLineBreaks = true;
                return this;
            }
        }

        public bool Succeeded
        {
            get => succeeded.HasValue && succeeded.Value;
        }

        public IAssertionScope BecauseOf(string because, params object[] becauseArgs)
        {
            reason = () =>
            {
                try
                {
                    string becauseOrEmpty = because ?? "";
                    return (becauseArgs?.Any() == true) ? string.Format(becauseOrEmpty, becauseArgs) : becauseOrEmpty;
                }
                catch (FormatException formatException)
                {
                    return $"**WARNING** because message '{because}' could not be formatted with string.Format{Environment.NewLine}{formatException.StackTrace}";
                }
            };
            return this;
        }

        /// <summary>
        /// Sets the expectation part of the failure message when the assertion is not met.
        /// </summary>
        /// <remarks>
        /// In addition to the numbered <see cref="string.Format(string,object[])"/>-style placeholders, messages may contain a few
        /// specialized placeholders as well. For instance, {reason} will be replaced with the reason of the assertion as passed
        /// to <see cref="BecauseOf"/>. Other named placeholders will be replaced with the <see cref="Current"/> scope data
        /// passed through <see cref="AddNonReportable"/> and <see cref="AddReportable"/>. Finally, a description of the
        /// current subject can be passed through the {context:description} placeholder. This is used in the message if no
        /// explicit context is specified through the <see cref="AssertionScope"/> constructor.
        /// Note that only 10 <paramref name="args"/> are supported in combination with a {reason}.
        /// If an expectation was set through a prior call to <see cref="WithExpectation"/>, then the failure message is appended to that
        /// expectation.
        /// </remarks>
        ///  <param name="message">The format string that represents the failure message.</param>
        /// <param name="args">Optional arguments to any numbered placeholders.</param>
        public IAssertionScope WithExpectation(string message, params object[] args)
        {
            var localReason = reason;
            expectation = () =>
            {
                var messageBuilder = new MessageBuilder(useLineBreaks);
                string reason = localReason?.Invoke() ?? "";
                string identifier = GetIdentifier();

                return messageBuilder.Build(message, args, reason, contextData, identifier, fallbackIdentifier);
            };

            return this;
        }

        public Continuation ClearExpectation()
        {
            expectation = null;

            return new Continuation(this, !succeeded.HasValue || succeeded.Value);
        }

        public GivenSelector<T> Given<T>(Func<T> selector)
        {
            return new GivenSelector<T>(selector, !succeeded.HasValue || succeeded.Value, this);
        }

        public IAssertionScope ForCondition(bool condition)
        {
            succeeded = condition;

            return this;
        }

        public Continuation FailWith(Func<FailReason> failReasonFunc)
        {
            try
            {
                if (!succeeded.HasValue || !succeeded.Value)
                {
                    string localReason = reason?.Invoke() ?? "";
                    var messageBuilder = new MessageBuilder(useLineBreaks);
                    string identifier = GetIdentifier();
                    var failReason = failReasonFunc();
                    string result = messageBuilder.Build(failReason.Message, failReason.Args, localReason, contextData, identifier, fallbackIdentifier);

                    if (expectation != null)
                    {
                        result = expectation() + result;
                    }

                    assertionStrategy.HandleFailure(result.Capitalize());

                    succeeded = false;
                }

                return new Continuation(this, succeeded.Value);
            }
            finally
            {
                succeeded = null;
            }
        }

        public Continuation FailWith(string message, params object[] args)
        {
            return FailWith(() => new FailReason(message, args));
        }

        private string GetIdentifier()
        {
            if (!string.IsNullOrEmpty(Context))
            {
                return Context;
            }

            return CallerIdentifier.DetermineCallerIdentity();
        }

        /// <summary>
        /// Adds a pre-formatted failure message to the current scope.
        /// </summary>
        public void AddPreFormattedFailure(string formattedFailureMessage)
        {
            assertionStrategy.HandleFailure(formattedFailureMessage);
        }

        public void AddNonReportable(string key, object value)
        {
            contextData.Add(key, value, Reportability.Hidden);
        }

        /// <summary>
        /// Adds some information to the assertion scope that will be included in the message
        /// that is emitted if an assertion fails.
        /// </summary>
        public void AddReportable(string key, string value)
        {
            contextData.Add(key, value, Reportability.Reportable);
        }

        public string[] Discard()
        {
            return assertionStrategy.DiscardFailures().ToArray();
        }

        public bool HasFailures()
        {
            return assertionStrategy.FailureMessages.Any();
        }

        /// <summary>
        /// Gets data associated with the current scope and identified by <paramref name="key"/>.
        /// </summary>
        public T Get<T>(string key)
        {
            return contextData.Get<T>(key);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Current = parent;

            if (parent != null)
            {
                foreach (string failureMessage in assertionStrategy.FailureMessages)
                {
                    parent.assertionStrategy.HandleFailure(failureMessage);
                }

                parent = null;
            }
            else
            {
                assertionStrategy.ThrowIfAny(contextData.GetReportable());
            }
        }

        public IAssertionScope WithDefaultIdentifier(string identifier)
        {
            fallbackIdentifier = identifier;
            return this;
        }
    }
}
