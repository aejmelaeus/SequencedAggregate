# SequencedAggregate
A simple Event Store that is using SQL Server as persistence

Using a traditional SQL database can be problematic when using a message based system. Messages can arrive in the wrong order, which is can be fatal in some situations.

Using a Event Store means that you don't have the current state of an object stored in a database, but all the event that lead to that state. This is helpful when working with message driven architectures. When a message arrives it is applied to the event stream in it's own sequence, meaning that it does not overwrite the latest state. Take for example the event `UserEmailUpdated`. If such a message is processed in wrong order the system will know the old email address.
