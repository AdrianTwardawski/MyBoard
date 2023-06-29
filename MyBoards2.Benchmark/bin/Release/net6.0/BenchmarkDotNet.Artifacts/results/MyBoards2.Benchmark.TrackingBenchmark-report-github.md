``` ini

BenchmarkDotNet=v0.13.5, OS=Windows 10 (10.0.19045.3086/22H2/2022Update)
AMD Ryzen 7 4800H with Radeon Graphics, 1 CPU, 16 logical and 8 physical cores
.NET SDK=6.0.401
  [Host]     : .NET 6.0.9 (6.0.922.41905), X64 RyuJIT AVX2 [AttachedDebugger]
  DefaultJob : .NET 6.0.9 (6.0.922.41905), X64 RyuJIT AVX2


```
|          Method |     Mean |     Error |    StdDev |     Gen0 |     Gen1 |  Allocated |
|---------------- |---------:|----------:|----------:|---------:|---------:|-----------:|
|    WithTracking | 2.802 ms | 0.0275 ms | 0.0257 ms | 242.1875 | 113.2813 | 1254.99 KB |
| WithoutTracking | 1.805 ms | 0.0291 ms | 0.0227 ms | 226.5625 |  66.4063 |  754.19 KB |
