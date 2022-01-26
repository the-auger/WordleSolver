# WordleSolver

A primitive Wordle solver.

I have yet to do any optimizations.

## Benchmark

``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.22000
AMD Ryzen 7 2700, 1 CPU, 16 logical and 8 physical cores
.NET SDK=6.0.101
  [Host]     : .NET 6.0.1 (6.0.121.56705), X64 RyuJIT
  Job-JHRHCT : .NET 6.0.1 (6.0.121.56705), X64 RyuJIT

InvocationCount=1  UnrollFactor=1  

```
| Method |  Word |           Mean |         Error |       StdDev |         Median | Ratio | Allocated |
|------- |------ |---------------:|--------------:|-------------:|---------------:|------:|----------:|
|  **Solve** | **AALST** |       **5.990 μs** |     **0.8950 μs** |     **2.611 μs** |       **4.850 μs** |  **1.00** |     **816 B** |
|        |       |                |               |              |                |       |           |
|  **Solve** | **ABAMA** |  **62,375.625 μs** | **1,511.8675 μs** | **4,457.775 μs** |  **61,680.950 μs** |  **1.00** | **281,008 B** |
|        |       |                |               |              |                |       |           |
|  **Solve** | **PEPSI** |  **11,546.459 μs** |   **410.6277 μs** | **1,210.745 μs** |  **12,322.500 μs** |  **1.00** | **269,680 B** |
|        |       |                |               |              |                |       |           |
|  **Solve** | **WORST** |   **4,462.488 μs** |    **87.7632 μs** |   **141.721 μs** |   **4,494.100 μs** |  **1.00** | **266,376 B** |
|        |       |                |               |              |                |       |           |
|  **Solve** | **ZYNGA** | **262,522.987 μs** | **3,327.5814 μs** | **3,112.622 μs** | **261,959.800 μs** |  **1.00** | **408,376 B** |
