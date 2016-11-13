# SequencedAggregate

The SequencedAggregate is a small library that sits on top ov `NEventStore` that handles Sequencing events the order they occured in another `Bounded Context`, not the time the events are handled.

[![Build status](https://ci.appveyor.com/api/projects/status/dxx6syjto963xq9c/branch/master?svg=true)](https://ci.appveyor.com/project/aejmelaeus/sequencedaggregate/branch/master)

## Driving forces

Aggregates within a `Bounded Context` can have parts of it's data owned by other bounded contexts and get updates to that data via events. A message based system is unreliable by nature and messages can arrive in wrong order.

Some events are immutable, then it usually don't matter if events arrive in the wrong order, for example `OrderCreated`. However when messages mutates a object, messages that arrive in the wrong order can have severe impact on the system. Take for example the message `UserEmailUpdated`. If those get mixed up, the system will start sending notifications to the old email address.

Traditional repositories that stores the latest snapshot of an object can have a hard time to deal with events that are delivered in the wrong order. In these cases a Event Stream might be a better case.

## Installing SequencedAggregate

You should install SequencedAggregate from NuGet (TODO!):

```
    Install-Package SequencedAggregate
```
## Documentation

Read more in the wiki (link).