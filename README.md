# SequencedAggregate

A simple Event Store that is using SQL Server as persistence

Using a Event Store means that you don't have the current state of an object stored in a database, but all the event that lead to that state. This is helpful when working with message driven architectures. When a message arrives it is applied to the event stream in it's own sequence, meaning that it does not overwrite the latest state.

Using a traditional SQL database can be problematic when using a message based system. Messages can arrive in the wrong order, which is can be fatal in some situations.

## Aggregate

The aggregate is "A collection of objects that are bound together by a root entity". Examples of aggregates in a system might be `Client`, `Order` or `User`. When you modify something within the aggregate, you persist the root object.

## Sequence

The sequence of events in a system can be critical, especially in cases where the events change the state of the same object. Take for example the event `UserEmailUpdated`. If such a message is processed in wrong order the system will know the old email address.

## Basic concepts

The state of your object is all events applied to it. When you read a object all events are applied to it. When you modify the object, new events are applyed to your object and stored.

## Usage

To create a aggregate root that is sequenced you derive from the `SequenceAggreagate` base class. This adds a few new functions to your class.