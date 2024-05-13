# Crystal Sharp - Message Broker RabbitMQ Example
Crystal Sharp framework - Message Broker code example with `RabbitMQ`.


### About This Example
This example uses `RabbitMQ` as a message broker.

`WebAPI` project sends the message to a message broker.
`Console` application receives the message from the message queue.


### How to Run

* `RabbitMQ` server must be running.
* Change the `RabbitMQ` settings in `appsettings.json` file in the `WebAPI` project.
* Change the `RabbitMQ` settings in `appsettings.json` file in the `Console` application.
* Change the `Queue` name in the `OrderController.cs` file.
* Change the `Queue` name in the `Console` application `Program.cs` file.
* `Queue` name must exist on the `RabbitMQ` server.
* Run the `WebAPI` project and `Console` application.
