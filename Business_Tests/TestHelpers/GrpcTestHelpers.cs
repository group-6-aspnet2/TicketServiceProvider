﻿using Business;
using Grpc.Core;

namespace Business_Tests.TestHelpers;
public static class GrpcTestHelpers
{
    public static AsyncUnaryCall<TResponse> CreateAsyncUnaryCall<TResponse>(TResponse response)
        where TResponse : class
    {
        return new AsyncUnaryCall<TResponse>(
            Task.FromResult(response),
            Task.FromResult(new Metadata()),
            () => Status.DefaultSuccess,
            () => new Metadata(),
            () => { });
    }
}

