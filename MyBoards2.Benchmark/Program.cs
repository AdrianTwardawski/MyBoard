// See https://aka.ms/new-console-template for more information
using BenchmarkDotNet.Running;
using MyBoards2.Benchmark;

Console.WriteLine("Hello, World!");

BenchmarkRunner.Run<TrackingBenchmark>();
