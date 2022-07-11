﻿#if !NETSTANDARD2_0

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAssertions.Common;

namespace FluentAssertions.Events;

/// <summary>
/// Tracks the events an object raises.
/// </summary>
internal sealed class EventMonitor<T> : IMonitor<T>
{
    private readonly WeakReference subject;

    private readonly ConcurrentDictionary<string, EventRecorder> recorderMap = new();

    public EventMonitor(object eventSource, EventMonitorOptions<T> options)
    {
        Guard.ThrowIfArgumentIsNull(eventSource, nameof(eventSource), "Cannot monitor the events of a <null> object.");

        this.options = options ?? new EventMonitorOptions<T>();

        subject = new WeakReference(eventSource);

        Attach(typeof(T), this.options.TimestampProvider);
    }

    public T Subject => (T)subject.Target;

    private readonly ThreadSafeSequenceGenerator threadSafeSequenceGenerator = new();
    private readonly EventMonitorOptions<T> options;

    public EventMetadata[] MonitoredEvents
    {
        get
        {
            return recorderMap
                .Values
                .Select(recorder => new EventMetadata(recorder.EventName, recorder.EventHandlerType))
                .ToArray();
        }
    }

    public OccurredEvent[] OccurredEvents
    {
        get
        {
            IEnumerable<OccurredEvent> query =
                from eventName in recorderMap.Keys
                let recording = GetRecordingFor(eventName)
                from @event in recording
                orderby @event.Sequence
                select @event;

            return query.ToArray();
        }
    }

    public void Clear()
    {
        foreach (EventRecorder recorder in recorderMap.Values)
        {
            recorder.Reset();
        }
    }

    public EventAssertions<T> Should()
    {
        return new EventAssertions<T>(this);
    }

    public IEventRecording GetRecordingFor(string eventName)
    {
        if (!recorderMap.TryGetValue(eventName, out EventRecorder recorder))
        {
            throw new InvalidOperationException($"Not monitoring any events named \"{eventName}\".");
        }

        return recorder;
    }

    private void Attach(Type typeDefiningEventsToMonitor, Func<DateTime> utcNow)
    {
        if (subject.Target is null)
        {
            throw new InvalidOperationException("Cannot monitor events on garbage-collected object");
        }

        EventInfo[] events = GetPublicEvents(typeDefiningEventsToMonitor);

        if (events.Length == 0)
        {
            throw new InvalidOperationException($"Type {typeDefiningEventsToMonitor.Name} does not expose any events.");
        }

        foreach (EventInfo eventInfo in events)
        {
            AttachEventHandler(eventInfo, utcNow);
        }
    }

    private static EventInfo[] GetPublicEvents(Type type)
    {
        if (!type.IsInterface)
        {
            return type.GetEvents();
        }

        return new[] { type }
            .Concat(type.GetInterfaces())
            .SelectMany(i => i.GetEvents())
            .ToArray();
    }

    public void Dispose()
    {
        foreach (EventRecorder recorder in recorderMap.Values)
        {
            DisposeSafeIfRequested(recorder);
        }

        recorderMap.Clear();
    }

    private void DisposeSafeIfRequested(EventRecorder recorder)
    {
        if (options.ShouldIgnoreEventAccessorExceptions)
        {
            try
            {
                recorder.Dispose();
            }
            catch
            {
                // ignore
            }
        }
        else
        {
            recorder.Dispose();
        }
    }

    private void AttachEventHandler(EventInfo eventInfo, Func<DateTime> utcNow)
    {
        if (!recorderMap.TryGetValue(eventInfo.Name, out _))
        {
            var recorder = new EventRecorder(subject.Target, eventInfo.Name, utcNow, threadSafeSequenceGenerator);

            if (recorderMap.TryAdd(eventInfo.Name, recorder))
            {
                AttachEventHandler(eventInfo, recorder);
            }
        }
    }

    private void AttachEventHandler(EventInfo eventInfo, EventRecorder recorder)
    {
        if (options.ShouldIgnoreEventAccessorExceptions)
        {
            AttachEventHandlerOrRemoveFromRecorderMapIfAttachmentFailed(eventInfo, recorder);
        }
        else
        {
            recorder.Attach(subject, eventInfo);
        }
    }

    private void AttachEventHandlerOrRemoveFromRecorderMapIfAttachmentFailed(EventInfo eventInfo, EventRecorder recorder)
    {
        try
        {
            recorder.Attach(subject, eventInfo);
        }
        catch
        {
            if (!recorderMap.TryRemove(eventInfo.Name, out _))
            {
                throw new InvalidOperationException(
                    $"Could not remove event {eventInfo.Name} with broken event accessor from event recording.");
            }
        }
    }
}

#endif
