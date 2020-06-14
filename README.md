# OrderSimulator (the project)

1. Development environment: 
    1. C# language, .NET Core 3.1
    1. Visual Studio 2019 
    1. Windows 10
    
1. To compile/run it, install visual Studio 2019(with .NET Core3.1) first (https://visualstudio.microsoft.com/vs/);

1. To run it, put the "orders.json" under the execution folder config/orders.json; and adjust the ingestion rate(orders per second) in the config.json.

1. the project is originally based on publisher-subscriber design pattern. And all the variables/classes/methods are named for self-explanation purpose. One publisher here ingests orders. And the subscribers (kitchen and courirer here) process the orders. Kitchen cooks the orders and places them in the shelf, while courier comes later(then the order is ready). This is a little tricky here: the subscribers have some relationship. 

1. Orders may be decayed with time, delivered by couriers or may be discarded when overflow shelf is full. Delegate method is applied here to deal with these cases. When couriers are ready, order has the notification tag. Order has a timer every second to check the self Value and the courier tag. With order deteriorates, the timer may trigger an event so that it will be removed from the corresponding shelf.

1. To avoid overwhelming log during every event output, the project print all the shelves information once there is an order palced. Between the "Order received/processed/placed" and ("Order delivered" or "Order decayed" or "Order discarded"), the shelves info print out log should contain this order.
