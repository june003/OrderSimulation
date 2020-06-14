# OrderSimulation (SIM)

1. this project is designed/developed in C# language with Visual Studio 2019 .NET Core 3.1 under Windows 10 environment. To compile/run it, install visual Studio 2019(with .NET Core3.1) first (https://visualstudio.microsoft.com/vs/)

2. SIM is originally based on publisher-subscriber design pattern. One publisher ingests orders. And the subscribers (kitchen and courirer here) process the orders. Kitchen cooks the order and places it in the shelf, while courier comes later(then the order is ready). There is a little tricky here: the subscribers have some relationship. Because order may be decay by itself, delivered by couriers or may be discarded when overflow shelf is full. Delegate method is applied here to trigger the events.

3. to avoid overwhelming log during every output, the project print all the shelves information once there is an order palced. Between the "Order received/processed/placed" and "Order delivered/decayed", all the shelves info print out log should contain this order.