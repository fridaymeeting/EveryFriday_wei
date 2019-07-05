Unix domain socket test
===

Basic test:

    dotnet run server &
    dotnet run client

Test to show client fails immediately when pipe doesn't exist (make sure /tmp/test_pipe is deleted, it isn't automatically deleted):

    dotnet run client
