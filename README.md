# guest_rabbitmq_sendmkdir CustomerBatchDemo
cd CustomerBatchDemo

# Tạo solution
dotnet new sln -n CustomerBatchDemo

# Tạo Web API (hệ thống A)
dotnet new console -n CustomerProducerApp

# Tạo Worker Service (hệ thống B)
dotnet new worker -n CustomerBatchDemo.Worker

# Thêm 2 project vào solution
dotnet sln add CustomerProducerApp/CustomerProducerApp.csproj
dotnet sln add CustomerBatchDemo.Worker/CustomerBatchDemo.Worker.csproj

# Thêm package RabbitMQ.Client
dotnet add package RabbitMQ.Client --version 6.4.0
dotnet add package Newtonsoft.Json