``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19044.1766 (21H2)
Intel Xeon CPU E5-2666 v3 2.90GHz, 1 CPU, 20 logical and 10 physical cores
.NET SDK=6.0.301
  [Host]   : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT
  ShortRun : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT

Job=ShortRun  Jit=RyuJit  Platform=X64  
Runtime=.NET 6.0  IterationCount=1  LaunchCount=1  
WarmupCount=1  

```
|                             Method |     Serializer |         Mean | Error | DataSize |  Gen 0 |  Gen 1 | Allocated |
|----------------------------------- |--------------- |-------------:|------:|---------:|-------:|-------:|----------:|
|          **_PrimitiveBoolDeserialize** | **MessagePack_v1** |     **28.86 ns** |    **NA** |        **-** |      **-** |      **-** |         **-** |
|          _PrimitiveBoolDeserialize | MessagePack_v2 |     64.70 ns |    NA |        - |      - |      - |         - |
|          _PrimitiveBoolDeserialize | MsgPack_v2_opt |     65.11 ns |    NA |        - |      - |      - |         - |
|          _PrimitiveBoolDeserialize |       SpanJson |     34.18 ns |    NA |        - |      - |      - |         - |
|          _PrimitiveBoolDeserialize | SystemTextJson |    114.97 ns |    NA |        - |      - |      - |         - |
|            **_PrimitiveBoolSerialize** | **MessagePack_v1** |     **50.53 ns** |    **NA** |      **1 B** | **0.0024** |      **-** |      **32 B** |
|            _PrimitiveBoolSerialize | MessagePack_v2 |     83.77 ns |    NA |      1 B | 0.0024 |      - |      32 B |
|            _PrimitiveBoolSerialize | MsgPack_v2_opt |     85.65 ns |    NA |      1 B | 0.0024 |      - |      32 B |
|            _PrimitiveBoolSerialize |       SpanJson |     52.52 ns |    NA |      5 B | 0.0024 |      - |      32 B |
|            _PrimitiveBoolSerialize | SystemTextJson |    131.04 ns |    NA |      5 B | 0.0141 |      - |     184 B |
|          **_PrimitiveByteDeserialize** | **MessagePack_v1** |     **28.74 ns** |    **NA** |        **-** |      **-** |      **-** |         **-** |
|          _PrimitiveByteDeserialize | MessagePack_v2 |     65.90 ns |    NA |        - |      - |      - |         - |
|          _PrimitiveByteDeserialize | MsgPack_v2_opt |     65.40 ns |    NA |        - |      - |      - |         - |
|          _PrimitiveByteDeserialize |       SpanJson |     31.98 ns |    NA |        - |      - |      - |         - |
|          _PrimitiveByteDeserialize | SystemTextJson |    116.66 ns |    NA |        - |      - |      - |         - |
|         **_PrimitiveBytesDeserialize** | **MessagePack_v1** |     **56.38 ns** |    **NA** |        **-** | **0.0098** |      **-** |     **128 B** |
|         _PrimitiveBytesDeserialize | MessagePack_v2 |    190.36 ns |    NA |        - | 0.0098 |      - |     128 B |
|         _PrimitiveBytesDeserialize | MsgPack_v2_opt |    213.52 ns |    NA |        - | 0.0098 |      - |     128 B |
|         _PrimitiveBytesDeserialize |       SpanJson |  1,502.38 ns |    NA |        - | 0.0095 |      - |     128 B |
|         _PrimitiveBytesDeserialize | SystemTextJson |    235.17 ns |    NA |        - | 0.0098 |      - |     128 B |
|            **_PrimitiveByteSerialize** | **MessagePack_v1** |     **50.21 ns** |    **NA** |      **2 B** | **0.0024** |      **-** |      **32 B** |
|            _PrimitiveByteSerialize | MessagePack_v2 |     81.19 ns |    NA |      2 B | 0.0024 |      - |      32 B |
|            _PrimitiveByteSerialize | MsgPack_v2_opt |     81.91 ns |    NA |      2 B | 0.0024 |      - |      32 B |
|            _PrimitiveByteSerialize |       SpanJson |     58.02 ns |    NA |      3 B | 0.0024 |      - |      32 B |
|            _PrimitiveByteSerialize | SystemTextJson |    149.04 ns |    NA |      3 B | 0.0141 |      - |     184 B |
|           **_PrimitiveBytesSerialize** | **MessagePack_v1** |     **73.76 ns** |    **NA** |    **102 B** | **0.0098** |      **-** |     **128 B** |
|           _PrimitiveBytesSerialize | MessagePack_v2 |    121.33 ns |    NA |    102 B | 0.0098 |      - |     128 B |
|           _PrimitiveBytesSerialize | MsgPack_v2_opt |    106.92 ns |    NA |    102 B | 0.0098 |      - |     128 B |
|           _PrimitiveBytesSerialize |       SpanJson |    958.93 ns |    NA |    351 B | 0.0286 |      - |     384 B |
|           _PrimitiveBytesSerialize | SystemTextJson |    210.04 ns |    NA |    138 B | 0.0243 |      - |     320 B |
|          **_PrimitiveCharDeserialize** | **MessagePack_v1** |     **29.57 ns** |    **NA** |        **-** |      **-** |      **-** |         **-** |
|          _PrimitiveCharDeserialize | MessagePack_v2 |     80.41 ns |    NA |        - |      - |      - |         - |
|          _PrimitiveCharDeserialize | MsgPack_v2_opt |     77.01 ns |    NA |        - |      - |      - |         - |
|          _PrimitiveCharDeserialize |       SpanJson |     50.28 ns |    NA |        - |      - |      - |         - |
|          _PrimitiveCharDeserialize | SystemTextJson |    245.99 ns |    NA |        - | 0.0017 |      - |      24 B |
|            **_PrimitiveCharSerialize** | **MessagePack_v1** |     **50.78 ns** |    **NA** |      **3 B** | **0.0024** |      **-** |      **32 B** |
|            _PrimitiveCharSerialize | MessagePack_v2 |     88.20 ns |    NA |      3 B | 0.0024 |      - |      32 B |
|            _PrimitiveCharSerialize | MsgPack_v2_opt |     85.89 ns |    NA |      3 B | 0.0024 |      - |      32 B |
|            _PrimitiveCharSerialize |       SpanJson |     77.62 ns |    NA |      5 B | 0.0024 |      - |      32 B |
|            _PrimitiveCharSerialize | SystemTextJson |    245.94 ns |    NA |      8 B | 0.0138 |      - |     184 B |
|      **_PrimitiveDateTimeDeserialize** | **MessagePack_v1** |     **43.69 ns** |    **NA** |        **-** |      **-** |      **-** |         **-** |
|      _PrimitiveDateTimeDeserialize | MessagePack_v2 |     95.92 ns |    NA |        - |      - |      - |         - |
|      _PrimitiveDateTimeDeserialize | MsgPack_v2_opt |     90.27 ns |    NA |        - |      - |      - |         - |
|      _PrimitiveDateTimeDeserialize |       SpanJson |    148.33 ns |    NA |        - |      - |      - |         - |
|      _PrimitiveDateTimeDeserialize | SystemTextJson |    230.80 ns |    NA |        - |      - |      - |         - |
|        **_PrimitiveDateTimeSerialize** | **MessagePack_v1** |     **95.93 ns** |    **NA** |     **15 B** | **0.0030** |      **-** |      **40 B** |
|        _PrimitiveDateTimeSerialize | MessagePack_v2 |    143.69 ns |    NA |     15 B | 0.0029 |      - |      40 B |
|        _PrimitiveDateTimeSerialize | MsgPack_v2_opt |    108.30 ns |    NA |      9 B | 0.0030 |      - |      40 B |
|        _PrimitiveDateTimeSerialize |       SpanJson |    139.95 ns |    NA |     32 B | 0.0041 |      - |      56 B |
|        _PrimitiveDateTimeSerialize | SystemTextJson |    206.89 ns |    NA |     32 B | 0.0157 |      - |     208 B |
|          **_PrimitiveGuidDeserialize** | **MessagePack_v1** |     **87.52 ns** |    **NA** |        **-** |      **-** |      **-** |         **-** |
|          _PrimitiveGuidDeserialize | MessagePack_v2 |    204.48 ns |    NA |        - |      - |      - |         - |
|          _PrimitiveGuidDeserialize | MsgPack_v2_opt |    159.33 ns |    NA |        - |      - |      - |         - |
|          _PrimitiveGuidDeserialize |       SpanJson |           NA |    NA |        - |      - |      - |         - |
|          _PrimitiveGuidDeserialize | SystemTextJson |    252.04 ns |    NA |        - |      - |      - |         - |
|            **_PrimitiveGuidSerialize** | **MessagePack_v1** |    **103.98 ns** |    **NA** |     **38 B** | **0.0049** |      **-** |      **64 B** |
|            _PrimitiveGuidSerialize | MessagePack_v2 |    133.80 ns |    NA |     38 B | 0.0048 |      - |      64 B |
|            _PrimitiveGuidSerialize | MsgPack_v2_opt |     90.61 ns |    NA |     18 B | 0.0036 |      - |      48 B |
|            _PrimitiveGuidSerialize |       SpanJson |     86.24 ns |    NA |     38 B | 0.0049 |      - |      64 B |
|            _PrimitiveGuidSerialize | SystemTextJson |    168.82 ns |    NA |     38 B | 0.0165 |      - |     216 B |
|           **_PrimitiveIntDeserialize** | **MessagePack_v1** |     **30.90 ns** |    **NA** |        **-** |      **-** |      **-** |         **-** |
|           _PrimitiveIntDeserialize | MessagePack_v2 |     72.62 ns |    NA |        - |      - |      - |         - |
|           _PrimitiveIntDeserialize | MsgPack_v2_opt |     73.68 ns |    NA |        - |      - |      - |         - |
|           _PrimitiveIntDeserialize |       SpanJson |     39.80 ns |    NA |        - |      - |      - |         - |
|           _PrimitiveIntDeserialize | SystemTextJson |    148.83 ns |    NA |        - |      - |      - |         - |
|             **_PrimitiveIntSerialize** | **MessagePack_v1** |     **59.74 ns** |    **NA** |      **5 B** | **0.0024** |      **-** |      **32 B** |
|             _PrimitiveIntSerialize | MessagePack_v2 |     84.44 ns |    NA |      5 B | 0.0024 |      - |      32 B |
|             _PrimitiveIntSerialize | MsgPack_v2_opt |     84.99 ns |    NA |      5 B | 0.0024 |      - |      32 B |
|             _PrimitiveIntSerialize |       SpanJson |     72.28 ns |    NA |      8 B | 0.0030 |      - |      40 B |
|             _PrimitiveIntSerialize | SystemTextJson |    144.92 ns |    NA |      8 B | 0.0145 |      - |     192 B |
|          **_PrimitiveLongDeserialize** | **MessagePack_v1** |     **32.21 ns** |    **NA** |        **-** |      **-** |      **-** |         **-** |
|          _PrimitiveLongDeserialize | MessagePack_v2 |     73.72 ns |    NA |        - |      - |      - |         - |
|          _PrimitiveLongDeserialize | MsgPack_v2_opt |     78.62 ns |    NA |        - |      - |      - |         - |
|          _PrimitiveLongDeserialize |       SpanJson |     60.17 ns |    NA |        - |      - |      - |         - |
|          _PrimitiveLongDeserialize | SystemTextJson |    201.52 ns |    NA |        - |      - |      - |         - |
|            **_PrimitiveLongSerialize** | **MessagePack_v1** |     **58.99 ns** |    **NA** |      **9 B** | **0.0030** |      **-** |      **40 B** |
|            _PrimitiveLongSerialize | MessagePack_v2 |     92.29 ns |    NA |      9 B | 0.0030 |      - |      40 B |
|            _PrimitiveLongSerialize | MsgPack_v2_opt |     91.76 ns |    NA |      9 B | 0.0030 |      - |      40 B |
|            _PrimitiveLongSerialize |       SpanJson |     79.85 ns |    NA |     19 B | 0.0036 |      - |      48 B |
|            _PrimitiveLongSerialize | SystemTextJson |    168.47 ns |    NA |     19 B | 0.0153 |      - |     200 B |
|         **_PrimitiveSByteDeserialize** | **MessagePack_v1** |     **28.20 ns** |    **NA** |        **-** |      **-** |      **-** |         **-** |
|         _PrimitiveSByteDeserialize | MessagePack_v2 |     74.88 ns |    NA |        - |      - |      - |         - |
|         _PrimitiveSByteDeserialize | MsgPack_v2_opt |     93.24 ns |    NA |        - |      - |      - |         - |
|         _PrimitiveSByteDeserialize |       SpanJson |     32.11 ns |    NA |        - |      - |      - |         - |
|         _PrimitiveSByteDeserialize | SystemTextJson |    118.44 ns |    NA |        - |      - |      - |         - |
|           **_PrimitiveSByteSerialize** | **MessagePack_v1** |     **49.85 ns** |    **NA** |      **1 B** | **0.0024** |      **-** |      **32 B** |
|           _PrimitiveSByteSerialize | MessagePack_v2 |     83.37 ns |    NA |      1 B | 0.0024 |      - |      32 B |
|           _PrimitiveSByteSerialize | MsgPack_v2_opt |     90.06 ns |    NA |      1 B | 0.0024 |      - |      32 B |
|           _PrimitiveSByteSerialize |       SpanJson |     57.71 ns |    NA |      3 B | 0.0024 |      - |      32 B |
|           _PrimitiveSByteSerialize | SystemTextJson |    127.79 ns |    NA |      3 B | 0.0141 |      - |     184 B |
|         **_PrimitiveShortDeserialize** | **MessagePack_v1** |     **30.52 ns** |    **NA** |        **-** |      **-** |      **-** |         **-** |
|         _PrimitiveShortDeserialize | MessagePack_v2 |     73.70 ns |    NA |        - |      - |      - |         - |
|         _PrimitiveShortDeserialize | MsgPack_v2_opt |     92.96 ns |    NA |        - |      - |      - |         - |
|         _PrimitiveShortDeserialize |       SpanJson |     34.63 ns |    NA |        - |      - |      - |         - |
|         _PrimitiveShortDeserialize | SystemTextJson |    120.97 ns |    NA |        - |      - |      - |         - |
|           **_PrimitiveShortSerialize** | **MessagePack_v1** |     **50.82 ns** |    **NA** |      **3 B** | **0.0024** |      **-** |      **32 B** |
|           _PrimitiveShortSerialize | MessagePack_v2 |     89.15 ns |    NA |      3 B | 0.0024 |      - |      32 B |
|           _PrimitiveShortSerialize | MsgPack_v2_opt |     91.94 ns |    NA |      3 B | 0.0024 |      - |      32 B |
|           _PrimitiveShortSerialize |       SpanJson |     69.71 ns |    NA |      5 B | 0.0024 |      - |      32 B |
|           _PrimitiveShortSerialize | SystemTextJson |    143.73 ns |    NA |      5 B | 0.0141 |      - |     184 B |
|        **_PrimitiveStringDeserialize** | **MessagePack_v1** |     **71.45 ns** |    **NA** |        **-** | **0.0030** |      **-** |      **40 B** |
|        _PrimitiveStringDeserialize | MessagePack_v2 |    113.38 ns |    NA |        - | 0.0029 |      - |      40 B |
|        _PrimitiveStringDeserialize | MsgPack_v2_opt |    119.45 ns |    NA |        - | 0.0029 |      - |      40 B |
|        _PrimitiveStringDeserialize |       SpanJson |     68.25 ns |    NA |        - | 0.0030 |      - |      40 B |
|        _PrimitiveStringDeserialize | SystemTextJson |    161.81 ns |    NA |        - | 0.0029 |      - |      40 B |
|          **_PrimitiveStringSerialize** | **MessagePack_v1** |     **85.65 ns** |    **NA** |      **9 B** | **0.0030** |      **-** |      **40 B** |
|          _PrimitiveStringSerialize | MessagePack_v2 |    110.73 ns |    NA |      9 B | 0.0030 |      - |      40 B |
|          _PrimitiveStringSerialize | MsgPack_v2_opt |    118.40 ns |    NA |      9 B | 0.0029 |      - |      40 B |
|          _PrimitiveStringSerialize |       SpanJson |    114.62 ns |    NA |     10 B | 0.0030 |      - |      40 B |
|          _PrimitiveStringSerialize | SystemTextJson |    175.71 ns |    NA |     10 B | 0.0145 |      - |     192 B |
|          **_PrimitiveUIntDeserialize** | **MessagePack_v1** |     **30.53 ns** |    **NA** |        **-** |      **-** |      **-** |         **-** |
|          _PrimitiveUIntDeserialize | MessagePack_v2 |     72.61 ns |    NA |        - |      - |      - |         - |
|          _PrimitiveUIntDeserialize | MsgPack_v2_opt |     72.84 ns |    NA |        - |      - |      - |         - |
|          _PrimitiveUIntDeserialize |       SpanJson |     40.83 ns |    NA |        - |      - |      - |         - |
|          _PrimitiveUIntDeserialize | SystemTextJson |    127.32 ns |    NA |        - |      - |      - |         - |
|            **_PrimitiveUIntSerialize** | **MessagePack_v1** |     **58.16 ns** |    **NA** |      **5 B** | **0.0024** |      **-** |      **32 B** |
|            _PrimitiveUIntSerialize | MessagePack_v2 |     86.03 ns |    NA |      5 B | 0.0024 |      - |      32 B |
|            _PrimitiveUIntSerialize | MsgPack_v2_opt |     83.04 ns |    NA |      5 B | 0.0024 |      - |      32 B |
|            _PrimitiveUIntSerialize |       SpanJson |     76.31 ns |    NA |     10 B | 0.0030 |      - |      40 B |
|            _PrimitiveUIntSerialize | SystemTextJson |    146.50 ns |    NA |     10 B | 0.0145 |      - |     192 B |
|         **_PrimitiveULongDeserialize** | **MessagePack_v1** |     **31.64 ns** |    **NA** |        **-** |      **-** |      **-** |         **-** |
|         _PrimitiveULongDeserialize | MessagePack_v2 |     75.78 ns |    NA |        - |      - |      - |         - |
|         _PrimitiveULongDeserialize | MsgPack_v2_opt |     73.40 ns |    NA |        - |      - |      - |         - |
|         _PrimitiveULongDeserialize |       SpanJson |     64.81 ns |    NA |        - |      - |      - |         - |
|         _PrimitiveULongDeserialize | SystemTextJson |    158.58 ns |    NA |        - |      - |      - |         - |
|           **_PrimitiveULongSerialize** | **MessagePack_v1** |     **58.66 ns** |    **NA** |      **9 B** | **0.0030** |      **-** |      **40 B** |
|           _PrimitiveULongSerialize | MessagePack_v2 |     90.79 ns |    NA |      9 B | 0.0030 |      - |      40 B |
|           _PrimitiveULongSerialize | MsgPack_v2_opt |     92.16 ns |    NA |      9 B | 0.0030 |      - |      40 B |
|           _PrimitiveULongSerialize |       SpanJson |     83.71 ns |    NA |     19 B | 0.0036 |      - |      48 B |
|           _PrimitiveULongSerialize | SystemTextJson |    158.57 ns |    NA |     19 B | 0.0153 |      - |     200 B |
|        **_PrimitiveUShortDeserialize** | **MessagePack_v1** |     **29.05 ns** |    **NA** |        **-** |      **-** |      **-** |         **-** |
|        _PrimitiveUShortDeserialize | MessagePack_v2 |     78.12 ns |    NA |        - |      - |      - |         - |
|        _PrimitiveUShortDeserialize | MsgPack_v2_opt |     81.67 ns |    NA |        - |      - |      - |         - |
|        _PrimitiveUShortDeserialize |       SpanJson |     31.26 ns |    NA |        - |      - |      - |         - |
|        _PrimitiveUShortDeserialize | SystemTextJson |    123.55 ns |    NA |        - |      - |      - |         - |
|          **_PrimitiveUShortSerialize** | **MessagePack_v1** |     **50.03 ns** |    **NA** |      **3 B** | **0.0024** |      **-** |      **32 B** |
|          _PrimitiveUShortSerialize | MessagePack_v2 |     83.51 ns |    NA |      3 B | 0.0024 |      - |      32 B |
|          _PrimitiveUShortSerialize | MsgPack_v2_opt |     82.02 ns |    NA |      3 B | 0.0024 |      - |      32 B |
|          _PrimitiveUShortSerialize |       SpanJson |     63.23 ns |    NA |      5 B | 0.0024 |      - |      32 B |
|          _PrimitiveUShortSerialize | SystemTextJson |    140.67 ns |    NA |      5 B | 0.0141 |      - |     184 B |
|             **AccessTokenDeserialize** | **MessagePack_v1** |    **282.61 ns** |    **NA** |        **-** | **0.0153** |      **-** |     **200 B** |
|             AccessTokenDeserialize | MessagePack_v2 |    376.50 ns |    NA |        - | 0.0153 |      - |     200 B |
|             AccessTokenDeserialize | MsgPack_v2_opt |    382.10 ns |    NA |        - | 0.0153 |      - |     200 B |
|             AccessTokenDeserialize |       SpanJson |    437.29 ns |    NA |        - | 0.0167 |      - |     224 B |
|             AccessTokenDeserialize | SystemTextJson |    963.16 ns |    NA |        - | 0.0477 |      - |     632 B |
|               **AccessTokenSerialize** | **MessagePack_v1** |    **313.01 ns** |    **NA** |     **40 B** | **0.0048** |      **-** |      **64 B** |
|               AccessTokenSerialize | MessagePack_v2 |    335.78 ns |    NA |     40 B | 0.0048 |      - |      64 B |
|               AccessTokenSerialize | MsgPack_v2_opt |    303.11 ns |    NA |     34 B | 0.0048 |      - |      64 B |
|               AccessTokenSerialize |       SpanJson |    333.51 ns |    NA |    122 B | 0.0114 |      - |     152 B |
|               AccessTokenSerialize | SystemTextJson |    675.20 ns |    NA |    122 B | 0.0486 |      - |     648 B |
|            **AccountMergeDeserialize** | **MessagePack_v1** |    **127.91 ns** |    **NA** |        **-** | **0.0036** |      **-** |      **48 B** |
|            AccountMergeDeserialize | MessagePack_v2 |    210.42 ns |    NA |        - | 0.0036 |      - |      48 B |
|            AccountMergeDeserialize | MsgPack_v2_opt |    215.74 ns |    NA |        - | 0.0033 |      - |      48 B |
|            AccountMergeDeserialize |       SpanJson |    287.18 ns |    NA |        - | 0.0033 |      - |      48 B |
|            AccountMergeDeserialize | SystemTextJson |    809.99 ns |    NA |        - | 0.0029 |      - |      48 B |
|              **AccountMergeSerialize** | **MessagePack_v1** |    **173.71 ns** |    **NA** |     **26 B** | **0.0041** |      **-** |      **56 B** |
|              AccountMergeSerialize | MessagePack_v2 |    207.86 ns |    NA |     26 B | 0.0038 |      - |      56 B |
|              AccountMergeSerialize | MsgPack_v2_opt |    188.10 ns |    NA |     20 B | 0.0036 |      - |      48 B |
|              AccountMergeSerialize |       SpanJson |    210.11 ns |    NA |    102 B | 0.0095 |      - |     128 B |
|              AccountMergeSerialize | SystemTextJson |    450.70 ns |    NA |    102 B | 0.0210 |      - |     280 B |
|                  **AnswerDeserialize** | **MessagePack_v1** |  **3,312.66 ns** |    **NA** |        **-** | **0.1373** |      **-** |   **1,816 B** |
|                  AnswerDeserialize | MessagePack_v2 |  3,709.34 ns |    NA |        - | 0.1373 |      - |   1,816 B |
|                  AnswerDeserialize | MsgPack_v2_opt |  3,695.02 ns |    NA |        - | 0.1373 |      - |   1,816 B |
|                  AnswerDeserialize |       SpanJson |  4,764.38 ns |    NA |        - | 0.1373 |      - |   1,864 B |
|                  AnswerDeserialize | SystemTextJson | 13,295.63 ns |    NA |        - | 0.1678 |      - |   2,272 B |
|                    **AnswerSerialize** | **MessagePack_v1** |  **2,721.62 ns** |    **NA** |    **470 B** | **0.0343** |      **-** |     **496 B** |
|                    AnswerSerialize | MessagePack_v2 |  2,675.26 ns |    NA |    470 B | 0.0343 |      - |     496 B |
|                    AnswerSerialize | MsgPack_v2_opt |  2,682.95 ns |    NA |    434 B | 0.0343 |      - |     464 B |
|                    AnswerSerialize |       SpanJson |  3,506.09 ns |    NA |   1843 B | 0.1411 |      - |   1,880 B |
|                    AnswerSerialize | SystemTextJson |  7,810.11 ns |    NA |   1790 B | 0.1678 |      - |   2,312 B |
|              **BadgeCountDeserialize** | **MessagePack_v1** |    **114.93 ns** |    **NA** |        **-** | **0.0029** |      **-** |      **40 B** |
|              BadgeCountDeserialize | MessagePack_v2 |    186.25 ns |    NA |        - | 0.0029 |      - |      40 B |
|              BadgeCountDeserialize | MsgPack_v2_opt |    184.20 ns |    NA |        - | 0.0029 |      - |      40 B |
|              BadgeCountDeserialize |       SpanJson |    172.62 ns |    NA |        - | 0.0029 |      - |      40 B |
|              BadgeCountDeserialize | SystemTextJson |    566.41 ns |    NA |        - | 0.0029 |      - |      40 B |
|                **BadgeCountSerialize** | **MessagePack_v1** |    **121.35 ns** |    **NA** |     **16 B** | **0.0029** |      **-** |      **40 B** |
|                BadgeCountSerialize | MessagePack_v2 |    159.24 ns |    NA |     16 B | 0.0029 |      - |      40 B |
|                BadgeCountSerialize | MsgPack_v2_opt |    170.30 ns |    NA |     16 B | 0.0029 |      - |      40 B |
|                BadgeCountSerialize |       SpanJson |    140.94 ns |    NA |     59 B | 0.0067 |      - |      88 B |
|                BadgeCountSerialize | SystemTextJson |    338.95 ns |    NA |     59 B | 0.0181 |      - |     240 B |
|                   **BadgeDeserialize** | **MessagePack_v1** |    **766.27 ns** |    **NA** |        **-** | **0.0334** |      **-** |     **440 B** |
|                   BadgeDeserialize | MessagePack_v2 |    923.66 ns |    NA |        - | 0.0334 |      - |     440 B |
|                   BadgeDeserialize | MsgPack_v2_opt |    971.63 ns |    NA |        - | 0.0324 |      - |     440 B |
|                   BadgeDeserialize |       SpanJson |  1,009.51 ns |    NA |        - | 0.0324 |      - |     440 B |
|                   BadgeDeserialize | SystemTextJson |  2,961.34 ns |    NA |        - | 0.0648 |      - |     848 B |
|                     **BadgeSerialize** | **MessagePack_v1** |    **634.29 ns** |    **NA** |    **101 B** | **0.0095** |      **-** |     **128 B** |
|                     BadgeSerialize | MessagePack_v2 |    731.18 ns |    NA |    101 B | 0.0095 |      - |     128 B |
|                     BadgeSerialize | MsgPack_v2_opt |    710.31 ns |    NA |    101 B | 0.0095 |      - |     128 B |
|                     BadgeSerialize |       SpanJson |    710.91 ns |    NA |    397 B | 0.0324 |      - |     424 B |
|                     BadgeSerialize | SystemTextJson |  1,809.86 ns |    NA |    369 B | 0.0668 |      - |     888 B |
|           **ClosedDetailsDeserialize** | **MessagePack_v1** |    **914.51 ns** |    **NA** |        **-** | **0.0448** |      **-** |     **592 B** |
|           ClosedDetailsDeserialize | MessagePack_v2 |  1,080.70 ns |    NA |        - | 0.0439 |      - |     592 B |
|           ClosedDetailsDeserialize | MsgPack_v2_opt |  1,107.50 ns |    NA |        - | 0.0439 |      - |     592 B |
|           ClosedDetailsDeserialize |       SpanJson |           NA |    NA |        - |      - |      - |         - |
|           ClosedDetailsDeserialize | SystemTextJson |  3,675.21 ns |    NA |        - | 0.0801 |      - |   1,048 B |
|             **ClosedDetailsSerialize** | **MessagePack_v1** |    **722.53 ns** |    **NA** |    **107 B** | **0.0095** |      **-** |     **136 B** |
|             ClosedDetailsSerialize | MessagePack_v2 |    814.57 ns |    NA |    107 B | 0.0095 |      - |     136 B |
|             ClosedDetailsSerialize | MsgPack_v2_opt |    782.07 ns |    NA |    107 B | 0.0095 |      - |     136 B |
|             ClosedDetailsSerialize |       SpanJson |    814.70 ns |    NA |    440 B | 0.0353 |      - |     472 B |
|             ClosedDetailsSerialize | SystemTextJson |  2,199.06 ns |    NA |    429 B | 0.0725 |      - |     952 B |
|                 **CommentDeserialize** | **MessagePack_v1** |  **1,496.25 ns** |    **NA** |        **-** | **0.0534** |      **-** |     **720 B** |
|                 CommentDeserialize | MessagePack_v2 |  1,683.79 ns |    NA |        - | 0.0534 |      - |     720 B |
|                 CommentDeserialize | MsgPack_v2_opt |  1,555.23 ns |    NA |        - | 0.0534 |      - |     720 B |
|                 CommentDeserialize |       SpanJson |  1,820.25 ns |    NA |        - | 0.0534 |      - |     720 B |
|                 CommentDeserialize | SystemTextJson |  5,386.19 ns |    NA |        - | 0.0839 |      - |   1,128 B |
|                   **CommentSerialize** | **MessagePack_v1** |  **1,116.30 ns** |    **NA** |    **181 B** | **0.0153** |      **-** |     **208 B** |
|                   CommentSerialize | MessagePack_v2 |  1,180.78 ns |    NA |    181 B | 0.0153 |      - |     208 B |
|                   CommentSerialize | MsgPack_v2_opt |  1,132.56 ns |    NA |    175 B | 0.0153 |      - |     200 B |
|                   CommentSerialize |       SpanJson |  1,255.21 ns |    NA |    728 B | 0.0572 |      - |     768 B |
|                   CommentSerialize | SystemTextJson |  3,092.54 ns |    NA |    698 B | 0.0916 |      - |   1,232 B |
|                   **ErrorDeserialize** | **MessagePack_v1** |    **197.06 ns** |    **NA** |        **-** | **0.0091** |      **-** |     **120 B** |
|                   ErrorDeserialize | MessagePack_v2 |    257.59 ns |    NA |        - | 0.0091 |      - |     120 B |
|                   ErrorDeserialize | MsgPack_v2_opt |    272.32 ns |    NA |        - | 0.0091 |      - |     120 B |
|                   ErrorDeserialize |       SpanJson |    231.00 ns |    NA |        - | 0.0091 |      - |     120 B |
|                   ErrorDeserialize | SystemTextJson |    573.22 ns |    NA |        - | 0.0086 |      - |     120 B |
|                     **ErrorSerialize** | **MessagePack_v1** |    **175.91 ns** |    **NA** |     **24 B** | **0.0036** |      **-** |      **48 B** |
|                     ErrorSerialize | MessagePack_v2 |    212.14 ns |    NA |     24 B | 0.0036 |      - |      48 B |
|                     ErrorSerialize | MsgPack_v2_opt |    210.76 ns |    NA |     24 B | 0.0036 |      - |      48 B |
|                     ErrorSerialize |       SpanJson |    219.67 ns |    NA |     71 B | 0.0072 |      - |      96 B |
|                     ErrorSerialize | SystemTextJson |    361.48 ns |    NA |     71 B | 0.0186 |      - |     248 B |
|                   **EventDeserialize** | **MessagePack_v1** |    **312.89 ns** |    **NA** |        **-** | **0.0110** |      **-** |     **144 B** |
|                   EventDeserialize | MessagePack_v2 |    360.83 ns |    NA |        - | 0.0110 |      - |     144 B |
|                   EventDeserialize | MsgPack_v2_opt |    361.38 ns |    NA |        - | 0.0110 |      - |     144 B |
|                   EventDeserialize |       SpanJson |    424.82 ns |    NA |        - | 0.0110 |      - |     144 B |
|                   EventDeserialize | SystemTextJson |    960.29 ns |    NA |        - | 0.0105 |      - |     144 B |
|                     **EventSerialize** | **MessagePack_v1** |    **276.05 ns** |    **NA** |     **40 B** | **0.0048** |      **-** |      **64 B** |
|                     EventSerialize | MessagePack_v2 |    306.19 ns |    NA |     40 B | 0.0048 |      - |      64 B |
|                     EventSerialize | MsgPack_v2_opt |    299.47 ns |    NA |     34 B | 0.0048 |      - |      64 B |
|                     EventSerialize |       SpanJson |    331.97 ns |    NA |    139 B | 0.0124 |      - |     168 B |
|                     EventSerialize | SystemTextJson |    537.80 ns |    NA |    125 B | 0.0229 |      - |     304 B |
|              **FlagOptionDeserialize** | **MessagePack_v1** |    **866.67 ns** |    **NA** |        **-** | **0.0305** |      **-** |     **400 B** |
|              FlagOptionDeserialize | MessagePack_v2 |    813.47 ns |    NA |        - | 0.0305 |      - |     400 B |
|              FlagOptionDeserialize | MsgPack_v2_opt |    828.01 ns |    NA |        - | 0.0305 |      - |     400 B |
|              FlagOptionDeserialize |       SpanJson |           NA |    NA |        - |      - |      - |         - |
|              FlagOptionDeserialize | SystemTextJson |  2,926.06 ns |    NA |        - | 0.0610 |      - |     832 B |
|                **FlagOptionSerialize** | **MessagePack_v1** |    **577.45 ns** |    **NA** |     **68 B** | **0.0067** |      **-** |      **96 B** |
|                FlagOptionSerialize | MessagePack_v2 |    603.18 ns |    NA |     68 B | 0.0067 |      - |      96 B |
|                FlagOptionSerialize | MsgPack_v2_opt |    632.43 ns |    NA |     68 B | 0.0067 |      - |      96 B |
|                FlagOptionSerialize |       SpanJson |    580.80 ns |    NA |    378 B | 0.0305 |      - |     400 B |
|                FlagOptionSerialize | SystemTextJson |  1,511.33 ns |    NA |    397 B | 0.0706 |      - |     928 B |
|               **InboxItemDeserialize** | **MessagePack_v1** |  **2,501.61 ns** |    **NA** |        **-** | **0.1068** |      **-** |   **1,408 B** |
|               InboxItemDeserialize | MessagePack_v2 |  2,219.49 ns |    NA |        - | 0.1068 |      - |   1,408 B |
|               InboxItemDeserialize | MsgPack_v2_opt |  2,220.93 ns |    NA |        - | 0.1068 |      - |   1,408 B |
|               InboxItemDeserialize |       SpanJson |  2,845.44 ns |    NA |        - | 0.1106 |      - |   1,480 B |
|               InboxItemDeserialize | SystemTextJson |  6,507.93 ns |    NA |        - | 0.1373 |      - |   1,888 B |
|                 **InboxItemSerialize** | **MessagePack_v1** |  **1,572.59 ns** |    **NA** |    **277 B** | **0.0229** |      **-** |     **304 B** |
|                 InboxItemSerialize | MessagePack_v2 |  1,799.83 ns |    NA |    277 B | 0.0229 |      - |     304 B |
|                 InboxItemSerialize | MsgPack_v2_opt |  1,580.10 ns |    NA |    253 B | 0.0191 |      - |     280 B |
|                 InboxItemSerialize |       SpanJson |  1,921.04 ns |    NA |    930 B | 0.0725 |      - |     960 B |
|                 InboxItemSerialize | SystemTextJson |  3,706.96 ns |    NA |    899 B | 0.1068 |      - |   1,424 B |
|                    **InfoDeserialize** | **MessagePack_v1** |  **2,487.84 ns** |    **NA** |        **-** | **0.1144** |      **-** |   **1,544 B** |
|                    InfoDeserialize | MessagePack_v2 |  2,731.88 ns |    NA |        - | 0.1068 |      - |   1,400 B |
|                    InfoDeserialize | MsgPack_v2_opt |  2,596.07 ns |    NA |        - | 0.1068 |      - |   1,400 B |
|                    InfoDeserialize |       SpanJson |  3,631.32 ns |    NA |        - | 0.1106 |      - |   1,472 B |
|                    InfoDeserialize | SystemTextJson |  8,959.53 ns |    NA |        - | 0.1373 |      - |   1,880 B |
|                      **InfoSerialize** | **MessagePack_v1** |  **2,028.54 ns** |    **NA** |    **313 B** | **0.0343** |      **-** |     **480 B** |
|                      InfoSerialize | MessagePack_v2 |  1,884.13 ns |    NA |    313 B | 0.0248 |      - |     344 B |
|                      InfoSerialize | MsgPack_v2_opt |  1,662.22 ns |    NA |    308 B | 0.0248 |      - |     336 B |
|                      InfoSerialize |       SpanJson |  2,201.30 ns |    NA |   1082 B | 0.0839 |      - |   1,112 B |
|                      InfoSerialize | SystemTextJson |  4,135.25 ns |    NA |   1070 B | 0.1144 |      - |   1,584 B |
|           **MigrationInfoDeserialize** | **MessagePack_v1** |  **1,683.46 ns** |    **NA** |        **-** | **0.0935** |      **-** |   **1,232 B** |
|           MigrationInfoDeserialize | MessagePack_v2 |  1,979.44 ns |    NA |        - | 0.0916 |      - |   1,232 B |
|           MigrationInfoDeserialize | MsgPack_v2_opt |  2,005.67 ns |    NA |        - | 0.0916 |      - |   1,232 B |
|           MigrationInfoDeserialize |       SpanJson |  2,374.39 ns |    NA |        - | 0.0992 |      - |   1,304 B |
|           MigrationInfoDeserialize | SystemTextJson |  5,578.98 ns |    NA |        - | 0.1297 |      - |   1,712 B |
|             **MigrationInfoSerialize** | **MessagePack_v1** |  **1,450.29 ns** |    **NA** |    **238 B** | **0.0191** |      **-** |     **264 B** |
|             MigrationInfoSerialize | MessagePack_v2 |  1,415.23 ns |    NA |    238 B | 0.0191 |      - |     264 B |
|             MigrationInfoSerialize | MsgPack_v2_opt |  1,548.95 ns |    NA |    214 B | 0.0172 |      - |     240 B |
|             MigrationInfoSerialize |       SpanJson |           NA |    NA |    783 B |      - |      - |         - |
|             MigrationInfoSerialize | SystemTextJson |  3,119.31 ns |    NA |    766 B | 0.0954 |      - |   1,288 B |
|  **MobileAssociationBonusDeserialize** | **MessagePack_v1** |    **204.04 ns** |    **NA** |        **-** | **0.0072** |      **-** |      **96 B** |
|  MobileAssociationBonusDeserialize | MessagePack_v2 |    257.64 ns |    NA |        - | 0.0072 |      - |      96 B |
|  MobileAssociationBonusDeserialize | MsgPack_v2_opt |    265.53 ns |    NA |        - | 0.0072 |      - |      96 B |
|  MobileAssociationBonusDeserialize |       SpanJson |    270.10 ns |    NA |        - | 0.0072 |      - |      96 B |
|  MobileAssociationBonusDeserialize | SystemTextJson |    736.54 ns |    NA |        - | 0.0067 |      - |      96 B |
|    **MobileAssociationBonusSerialize** | **MessagePack_v1** |    **186.48 ns** |    **NA** |     **29 B** | **0.0041** |      **-** |      **56 B** |
|    MobileAssociationBonusSerialize | MessagePack_v2 |    221.17 ns |    NA |     29 B | 0.0041 |      - |      56 B |
|    MobileAssociationBonusSerialize | MsgPack_v2_opt |    215.65 ns |    NA |     29 B | 0.0041 |      - |      56 B |
|    MobileAssociationBonusSerialize |       SpanJson |    275.93 ns |    NA |     92 B | 0.0091 |      - |     120 B |
|    MobileAssociationBonusSerialize | SystemTextJson |    428.55 ns |    NA |     92 B | 0.0205 |      - |     272 B |
|        **MobileBadgeAwardDeserialize** | **MessagePack_v1** |    **455.79 ns** |    **NA** |        **-** | **0.0200** |      **-** |     **264 B** |
|        MobileBadgeAwardDeserialize | MessagePack_v2 |    563.88 ns |    NA |        - | 0.0200 |      - |     264 B |
|        MobileBadgeAwardDeserialize | MsgPack_v2_opt |    584.20 ns |    NA |        - | 0.0200 |      - |     264 B |
|        MobileBadgeAwardDeserialize |       SpanJson |    668.75 ns |    NA |        - | 0.0200 |      - |     264 B |
|        MobileBadgeAwardDeserialize | SystemTextJson |  1,796.71 ns |    NA |        - | 0.0191 |      - |     264 B |
|          **MobileBadgeAwardSerialize** | **MessagePack_v1** |    **395.76 ns** |    **NA** |     **63 B** | **0.0067** |      **-** |      **88 B** |
|          MobileBadgeAwardSerialize | MessagePack_v2 |    443.53 ns |    NA |     63 B | 0.0067 |      - |      88 B |
|          MobileBadgeAwardSerialize | MsgPack_v2_opt |    456.49 ns |    NA |     63 B | 0.0067 |      - |      88 B |
|          MobileBadgeAwardSerialize |       SpanJson |    464.38 ns |    NA |    225 B | 0.0191 |      - |     256 B |
|          MobileBadgeAwardSerialize | SystemTextJson |    878.05 ns |    NA |    212 B | 0.0296 |      - |     392 B |
|          **MobileBannerAdDeserialize** | **MessagePack_v1** |    **395.32 ns** |    **NA** |        **-** | **0.0181** |      **-** |     **240 B** |
|          MobileBannerAdDeserialize | MessagePack_v2 |    474.81 ns |    NA |        - | 0.0181 |      - |     240 B |
|          MobileBannerAdDeserialize | MsgPack_v2_opt |    557.83 ns |    NA |        - | 0.0181 |      - |     240 B |
|          MobileBannerAdDeserialize |       SpanJson |    526.59 ns |    NA |        - | 0.0200 |      - |     264 B |
|          MobileBannerAdDeserialize | SystemTextJson |  1,411.48 ns |    NA |        - | 0.0496 |      - |     672 B |
|     **MobileBannerAdImageDeserialize** | **MessagePack_v1** |    **167.97 ns** |    **NA** |        **-** | **0.0060** |      **-** |      **80 B** |
|     MobileBannerAdImageDeserialize | MessagePack_v2 |    233.07 ns |    NA |        - | 0.0057 |      - |      80 B |
|     MobileBannerAdImageDeserialize | MsgPack_v2_opt |    250.11 ns |    NA |        - | 0.0060 |      - |      80 B |
|     MobileBannerAdImageDeserialize |       SpanJson |    205.30 ns |    NA |        - | 0.0060 |      - |      80 B |
|     MobileBannerAdImageDeserialize | SystemTextJson |    581.86 ns |    NA |        - | 0.0057 |      - |      80 B |
|       **MobileBannerAdImageSerialize** | **MessagePack_v1** |    **157.82 ns** |    **NA** |     **20 B** | **0.0036** |      **-** |      **48 B** |
|       MobileBannerAdImageSerialize | MessagePack_v2 |    195.15 ns |    NA |     20 B | 0.0036 |      - |      48 B |
|       MobileBannerAdImageSerialize | MsgPack_v2_opt |    191.94 ns |    NA |     20 B | 0.0036 |      - |      48 B |
|       MobileBannerAdImageSerialize |       SpanJson |    198.92 ns |    NA |     62 B | 0.0067 |      - |      88 B |
|       MobileBannerAdImageSerialize | SystemTextJson |    359.75 ns |    NA |     62 B | 0.0181 |      - |     240 B |
|            **MobileBannerAdSerialize** | **MessagePack_v1** |    **310.92 ns** |    **NA** |     **45 B** | **0.0052** |      **-** |      **72 B** |
|            MobileBannerAdSerialize | MessagePack_v2 |    345.12 ns |    NA |     45 B | 0.0052 |      - |      72 B |
|            MobileBannerAdSerialize | MsgPack_v2_opt |    343.54 ns |    NA |     45 B | 0.0052 |      - |      72 B |
|            MobileBannerAdSerialize |       SpanJson |    364.64 ns |    NA |    149 B | 0.0134 |      - |     176 B |
|            MobileBannerAdSerialize | SystemTextJson |    953.36 ns |    NA |    149 B | 0.0505 |      - |     672 B |
|      **MobileCareersJobAdDeserialize** | **MessagePack_v1** |    **384.26 ns** |    **NA** |        **-** | **0.0181** |      **-** |     **240 B** |
|      MobileCareersJobAdDeserialize | MessagePack_v2 |    459.66 ns |    NA |        - | 0.0181 |      - |     240 B |
|      MobileCareersJobAdDeserialize | MsgPack_v2_opt |    459.29 ns |    NA |        - | 0.0181 |      - |     240 B |
|      MobileCareersJobAdDeserialize |       SpanJson |    507.28 ns |    NA |        - | 0.0181 |      - |     240 B |
|      MobileCareersJobAdDeserialize | SystemTextJson |  1,184.68 ns |    NA |        - | 0.0172 |      - |     240 B |
|        **MobileCareersJobAdSerialize** | **MessagePack_v1** |    **323.29 ns** |    **NA** |     **56 B** | **0.0057** |      **-** |      **80 B** |
|        MobileCareersJobAdSerialize | MessagePack_v2 |    350.30 ns |    NA |     56 B | 0.0057 |      - |      80 B |
|        MobileCareersJobAdSerialize | MsgPack_v2_opt |    357.12 ns |    NA |     56 B | 0.0057 |      - |      80 B |
|        MobileCareersJobAdSerialize |       SpanJson |    409.88 ns |    NA |    160 B | 0.0138 |      - |     184 B |
|        MobileCareersJobAdSerialize | SystemTextJson |    718.22 ns |    NA |    160 B | 0.0248 |      - |     336 B |
| **MobileCommunityBulletinDeserialize** | **MessagePack_v1** |    **639.52 ns** |    **NA** |        **-** | **0.0315** |      **-** |     **416 B** |
| MobileCommunityBulletinDeserialize | MessagePack_v2 |    828.50 ns |    NA |        - | 0.0315 |      - |     416 B |
| MobileCommunityBulletinDeserialize | MsgPack_v2_opt |    831.81 ns |    NA |        - | 0.0315 |      - |     416 B |
| MobileCommunityBulletinDeserialize |       SpanJson |    948.48 ns |    NA |        - | 0.0334 |      - |     440 B |
| MobileCommunityBulletinDeserialize | SystemTextJson |  2,277.31 ns |    NA |        - | 0.0648 |      - |     848 B |
|   **MobileCommunityBulletinSerialize** | **MessagePack_v1** |    **529.90 ns** |    **NA** |     **88 B** | **0.0076** |      **-** |     **112 B** |
|   MobileCommunityBulletinSerialize | MessagePack_v2 |    577.73 ns |    NA |     88 B | 0.0076 |      - |     112 B |
|   MobileCommunityBulletinSerialize | MsgPack_v2_opt |    592.90 ns |    NA |     88 B | 0.0076 |      - |     112 B |
|   MobileCommunityBulletinSerialize |       SpanJson |    614.60 ns |    NA |    356 B | 0.0286 |      - |     376 B |
|   MobileCommunityBulletinSerialize | SystemTextJson |  1,449.69 ns |    NA |    333 B | 0.0648 |      - |     864 B |
|              **MobileFeedDeserialize** | **MessagePack_v1** |  **7,000.86 ns** |    **NA** |        **-** | **0.3204** | **0.0076** |   **4,248 B** |
|              MobileFeedDeserialize | MessagePack_v2 |  8,074.03 ns |    NA |        - | 0.3204 | 0.0076 |   4,248 B |
|              MobileFeedDeserialize | MsgPack_v2_opt |  7,527.84 ns |    NA |        - | 0.3204 |      - |   4,248 B |
|              MobileFeedDeserialize |       SpanJson |  8,567.58 ns |    NA |        - | 0.3510 |      - |   4,656 B |
|              MobileFeedDeserialize | SystemTextJson | 24,889.09 ns |    NA |        - | 0.3662 |      - |   5,064 B |
|                **MobileFeedSerialize** | **MessagePack_v1** |  **4,947.85 ns** |    **NA** |    **811 B** | **0.0610** |      **-** |     **840 B** |
|                MobileFeedSerialize | MessagePack_v2 |  4,951.14 ns |    NA |    811 B | 0.0610 |      - |     840 B |
|                MobileFeedSerialize | MsgPack_v2_opt |  5,178.53 ns |    NA |    811 B | 0.0610 |      - |     840 B |
|                MobileFeedSerialize |       SpanJson |  5,601.78 ns |    NA |   3167 B | 0.2365 |      - |   3,184 B |
|                MobileFeedSerialize | SystemTextJson | 14,422.43 ns |    NA |   3140 B | 0.2747 |      - |   3,664 B |
|         **MobileInboxItemDeserialize** | **MessagePack_v1** |    **505.08 ns** |    **NA** |        **-** | **0.0238** |      **-** |     **320 B** |
|         MobileInboxItemDeserialize | MessagePack_v2 |    628.68 ns |    NA |        - | 0.0238 |      - |     320 B |
|         MobileInboxItemDeserialize | MsgPack_v2_opt |    636.74 ns |    NA |        - | 0.0238 |      - |     320 B |
|         MobileInboxItemDeserialize |       SpanJson |    680.71 ns |    NA |        - | 0.0238 |      - |     320 B |
|         MobileInboxItemDeserialize | SystemTextJson |  1,890.14 ns |    NA |        - | 0.0229 |      - |     320 B |
|           **MobileInboxItemSerialize** | **MessagePack_v1** |    **500.36 ns** |    **NA** |     **84 B** | **0.0076** |      **-** |     **112 B** |
|           MobileInboxItemSerialize | MessagePack_v2 |    472.98 ns |    NA |     84 B | 0.0076 |      - |     112 B |
|           MobileInboxItemSerialize | MsgPack_v2_opt |    484.89 ns |    NA |     84 B | 0.0076 |      - |     112 B |
|           MobileInboxItemSerialize |       SpanJson |    556.40 ns |    NA |    256 B | 0.0219 |      - |     288 B |
|           MobileInboxItemSerialize | SystemTextJson |  1,022.88 ns |    NA |    256 B | 0.0324 |      - |     440 B |
|         **MobilePrivilegeDeserialize** | **MessagePack_v1** |    **401.07 ns** |    **NA** |        **-** | **0.0186** |      **-** |     **248 B** |
|         MobilePrivilegeDeserialize | MessagePack_v2 |    504.42 ns |    NA |        - | 0.0181 |      - |     248 B |
|         MobilePrivilegeDeserialize | MsgPack_v2_opt |    512.35 ns |    NA |        - | 0.0181 |      - |     248 B |
|         MobilePrivilegeDeserialize |       SpanJson |    514.26 ns |    NA |        - | 0.0181 |      - |     248 B |
|         MobilePrivilegeDeserialize | SystemTextJson |  1,366.25 ns |    NA |        - | 0.0172 |      - |     248 B |
|           **MobilePrivilegeSerialize** | **MessagePack_v1** |    **339.40 ns** |    **NA** |     **61 B** | **0.0067** |      **-** |      **88 B** |
|           MobilePrivilegeSerialize | MessagePack_v2 |    366.03 ns |    NA |     61 B | 0.0067 |      - |      88 B |
|           MobilePrivilegeSerialize | MsgPack_v2_opt |    433.28 ns |    NA |     61 B | 0.0067 |      - |      88 B |
|           MobilePrivilegeSerialize |       SpanJson |    426.10 ns |    NA |    231 B | 0.0196 |      - |     256 B |
|           MobilePrivilegeSerialize | SystemTextJson |    817.73 ns |    NA |    231 B | 0.0305 |      - |     408 B |
|          **MobileQuestionDeserialize** | **MessagePack_v1** |    **557.67 ns** |    **NA** |        **-** | **0.0238** |      **-** |     **312 B** |
|          MobileQuestionDeserialize | MessagePack_v2 |    607.05 ns |    NA |        - | 0.0238 |      - |     312 B |
|          MobileQuestionDeserialize | MsgPack_v2_opt |    726.71 ns |    NA |        - | 0.0238 |      - |     312 B |
|          MobileQuestionDeserialize |       SpanJson |    667.36 ns |    NA |        - | 0.0248 |      - |     336 B |
|          MobileQuestionDeserialize | SystemTextJson |  1,885.33 ns |    NA |        - | 0.0534 |      - |     744 B |
|            **MobileQuestionSerialize** | **MessagePack_v1** |    **429.59 ns** |    **NA** |     **73 B** | **0.0076** |      **-** |     **104 B** |
|            MobileQuestionSerialize | MessagePack_v2 |    463.52 ns |    NA |     73 B | 0.0076 |      - |     104 B |
|            MobileQuestionSerialize | MsgPack_v2_opt |    509.59 ns |    NA |     73 B | 0.0076 |      - |     104 B |
|            MobileQuestionSerialize |       SpanJson |    481.96 ns |    NA |    292 B | 0.0238 |      - |     320 B |
|            MobileQuestionSerialize | SystemTextJson |  1,113.35 ns |    NA |    292 B | 0.0610 |      - |     816 B |
|         **MobileRepChangeDeserialize** | **MessagePack_v1** |    **307.70 ns** |    **NA** |        **-** | **0.0143** |      **-** |     **192 B** |
|         MobileRepChangeDeserialize | MessagePack_v2 |    400.93 ns |    NA |        - | 0.0143 |      - |     192 B |
|         MobileRepChangeDeserialize | MsgPack_v2_opt |    397.74 ns |    NA |        - | 0.0143 |      - |     192 B |
|         MobileRepChangeDeserialize |       SpanJson |    400.26 ns |    NA |        - | 0.0143 |      - |     192 B |
|         MobileRepChangeDeserialize | SystemTextJson |  1,015.68 ns |    NA |        - | 0.0134 |      - |     192 B |
|           **MobileRepChangeSerialize** | **MessagePack_v1** |    **305.65 ns** |    **NA** |     **47 B** | **0.0052** |      **-** |      **72 B** |
|           MobileRepChangeSerialize | MessagePack_v2 |    303.48 ns |    NA |     47 B | 0.0052 |      - |      72 B |
|           MobileRepChangeSerialize | MsgPack_v2_opt |    307.87 ns |    NA |     47 B | 0.0052 |      - |      72 B |
|           MobileRepChangeSerialize |       SpanJson |    344.02 ns |    NA |    135 B | 0.0119 |      - |     160 B |
|           MobileRepChangeSerialize | SystemTextJson |    640.11 ns |    NA |    135 B | 0.0238 |      - |     312 B |
|      **MobileUpdateNoticeDeserialize** | **MessagePack_v1** |    **199.46 ns** |    **NA** |        **-** | **0.0091** |      **-** |     **120 B** |
|      MobileUpdateNoticeDeserialize | MessagePack_v2 |    241.39 ns |    NA |        - | 0.0091 |      - |     120 B |
|      MobileUpdateNoticeDeserialize | MsgPack_v2_opt |    245.43 ns |    NA |        - | 0.0091 |      - |     120 B |
|      MobileUpdateNoticeDeserialize |       SpanJson |    217.08 ns |    NA |        - | 0.0091 |      - |     120 B |
|      MobileUpdateNoticeDeserialize | SystemTextJson |    589.62 ns |    NA |        - | 0.0086 |      - |     120 B |
|        **MobileUpdateNoticeSerialize** | **MessagePack_v1** |    **171.09 ns** |    **NA** |     **20 B** | **0.0036** |      **-** |      **48 B** |
|        MobileUpdateNoticeSerialize | MessagePack_v2 |    206.93 ns |    NA |     20 B | 0.0036 |      - |      48 B |
|        MobileUpdateNoticeSerialize | MsgPack_v2_opt |    208.14 ns |    NA |     20 B | 0.0036 |      - |      48 B |
|        MobileUpdateNoticeSerialize |       SpanJson |    195.57 ns |    NA |     82 B | 0.0083 |      - |     112 B |
|        MobileUpdateNoticeSerialize | SystemTextJson |    367.85 ns |    NA |     82 B | 0.0200 |      - |     264 B |
|             **NetworkUserDeserialize** | **MessagePack_v1** |    **505.00 ns** |    **NA** |        **-** | **0.0181** |      **-** |     **240 B** |
|             NetworkUserDeserialize | MessagePack_v2 |    665.94 ns |    NA |        - | 0.0181 |      - |     240 B |
|             NetworkUserDeserialize | MsgPack_v2_opt |    754.12 ns |    NA |        - | 0.0181 |      - |     240 B |
|             NetworkUserDeserialize |       SpanJson |    917.55 ns |    NA |        - | 0.0172 |      - |     240 B |
|             NetworkUserDeserialize | SystemTextJson |  2,389.65 ns |    NA |        - | 0.0458 |      - |     648 B |
|               **NetworkUserSerialize** | **MessagePack_v1** |    **533.76 ns** |    **NA** |     **91 B** | **0.0086** |      **-** |     **120 B** |
|               NetworkUserSerialize | MessagePack_v2 |    542.37 ns |    NA |     91 B | 0.0086 |      - |     120 B |
|               NetworkUserSerialize | MsgPack_v2_opt |    517.91 ns |    NA |     79 B | 0.0076 |      - |     104 B |
|               NetworkUserSerialize |       SpanJson |    624.08 ns |    NA |    367 B | 0.0286 |      - |     384 B |
|               NetworkUserSerialize | SystemTextJson |  1,489.07 ns |    NA |    356 B | 0.0668 |      - |     880 B |
|                  **NoticeDeserialize** | **MessagePack_v1** |    **186.43 ns** |    **NA** |        **-** | **0.0067** |      **-** |      **88 B** |
|                  NoticeDeserialize | MessagePack_v2 |    257.47 ns |    NA |        - | 0.0067 |      - |      88 B |
|                  NoticeDeserialize | MsgPack_v2_opt |    261.39 ns |    NA |        - | 0.0067 |      - |      88 B |
|                  NoticeDeserialize |       SpanJson |    303.44 ns |    NA |        - | 0.0067 |      - |      88 B |
|                  NoticeDeserialize | SystemTextJson |    636.48 ns |    NA |        - | 0.0067 |      - |      88 B |
|                    **NoticeSerialize** | **MessagePack_v1** |    **205.58 ns** |    **NA** |     **30 B** | **0.0041** |      **-** |      **56 B** |
|                    NoticeSerialize | MessagePack_v2 |    235.95 ns |    NA |     30 B | 0.0038 |      - |      56 B |
|                    NoticeSerialize | MsgPack_v2_opt |    223.51 ns |    NA |     24 B | 0.0036 |      - |      48 B |
|                    NoticeSerialize |       SpanJson |    264.21 ns |    NA |     94 B | 0.0091 |      - |     120 B |
|                    NoticeSerialize | SystemTextJson |    428.35 ns |    NA |     94 B | 0.0205 |      - |     272 B |
|            **NotificationDeserialize** | **MessagePack_v1** |  **1,926.17 ns** |    **NA** |        **-** | **0.0973** |      **-** |   **1,296 B** |
|            NotificationDeserialize | MessagePack_v2 |  2,069.18 ns |    NA |        - | 0.0954 |      - |   1,296 B |
|            NotificationDeserialize | MsgPack_v2_opt |  2,017.36 ns |    NA |        - | 0.0954 |      - |   1,296 B |
|            NotificationDeserialize |       SpanJson |  2,818.13 ns |    NA |        - | 0.1030 |      - |   1,368 B |
|            NotificationDeserialize | SystemTextJson |  6,066.02 ns |    NA |        - | 0.1297 |      - |   1,776 B |
|              **NotificationSerialize** | **MessagePack_v1** |  **1,477.91 ns** |    **NA** |    **249 B** | **0.0210** |      **-** |     **280 B** |
|              NotificationSerialize | MessagePack_v2 |  1,476.37 ns |    NA |    249 B | 0.0210 |      - |     280 B |
|              NotificationSerialize | MsgPack_v2_opt |  1,493.68 ns |    NA |    225 B | 0.0191 |      - |     256 B |
|              NotificationSerialize |       SpanJson |  1,742.77 ns |    NA |    846 B | 0.0668 |      - |     888 B |
|              NotificationSerialize | SystemTextJson |  3,451.07 ns |    NA |    821 B | 0.0992 |      - |   1,344 B |
|        **OriginalQuestionDeserialize** | **MessagePack_v1** |    **180.33 ns** |    **NA** |        **-** | **0.0067** |      **-** |      **88 B** |
|        OriginalQuestionDeserialize | MessagePack_v2 |    257.85 ns |    NA |        - | 0.0067 |      - |      88 B |
|        OriginalQuestionDeserialize | MsgPack_v2_opt |    253.60 ns |    NA |        - | 0.0067 |      - |      88 B |
|        OriginalQuestionDeserialize |       SpanJson |    245.16 ns |    NA |        - | 0.0067 |      - |      88 B |
|        OriginalQuestionDeserialize | SystemTextJson |    712.03 ns |    NA |        - | 0.0067 |      - |      88 B |
|          **OriginalQuestionSerialize** | **MessagePack_v1** |    **198.15 ns** |    **NA** |     **25 B** | **0.0038** |      **-** |      **56 B** |
|          OriginalQuestionSerialize | MessagePack_v2 |    202.70 ns |    NA |     25 B | 0.0041 |      - |      56 B |
|          OriginalQuestionSerialize | MsgPack_v2_opt |    210.80 ns |    NA |     25 B | 0.0041 |      - |      56 B |
|          OriginalQuestionSerialize |       SpanJson |    221.21 ns |    NA |    101 B | 0.0098 |      - |     128 B |
|          OriginalQuestionSerialize | SystemTextJson |    430.66 ns |    NA |    101 B | 0.0210 |      - |     280 B |
|                    **PostDeserialize** | **MessagePack_v1** |  **2,927.62 ns** |    **NA** |        **-** | **0.1221** |      **-** |   **1,608 B** |
|                    PostDeserialize | MessagePack_v2 |  3,652.79 ns |    NA |        - | 0.1221 |      - |   1,608 B |
|                    PostDeserialize | MsgPack_v2_opt |  3,485.03 ns |    NA |        - | 0.1221 |      - |   1,608 B |
|                    PostDeserialize |       SpanJson |  4,280.21 ns |    NA |        - | 0.1221 |      - |   1,632 B |
|                    PostDeserialize | SystemTextJson | 14,680.34 ns |    NA |        - | 0.1526 |      - |   2,040 B |
|                      **PostSerialize** | **MessagePack_v1** |  **2,407.74 ns** |    **NA** |    **415 B** | **0.0305** |      **-** |     **440 B** |
|                      PostSerialize | MessagePack_v2 |  2,405.81 ns |    NA |    415 B | 0.0305 |      - |     440 B |
|                      PostSerialize | MsgPack_v2_opt |  2,373.93 ns |    NA |    391 B | 0.0305 |      - |     416 B |
|                      PostSerialize |       SpanJson |  2,830.05 ns |    NA |   1665 B | 0.1297 |      - |   1,696 B |
|                      PostSerialize | SystemTextJson |  6,983.83 ns |    NA |   1602 B | 0.1602 |      - |   2,128 B |
|               **PrivilegeDeserialize** | **MessagePack_v1** |    **222.14 ns** |    **NA** |        **-** | **0.0091** |      **-** |     **120 B** |
|               PrivilegeDeserialize | MessagePack_v2 |    272.31 ns |    NA |        - | 0.0091 |      - |     120 B |
|               PrivilegeDeserialize | MsgPack_v2_opt |    266.79 ns |    NA |        - | 0.0091 |      - |     120 B |
|               PrivilegeDeserialize |       SpanJson |    232.49 ns |    NA |        - | 0.0091 |      - |     120 B |
|               PrivilegeDeserialize | SystemTextJson |    582.71 ns |    NA |        - | 0.0086 |      - |     120 B |
|                 **PrivilegeSerialize** | **MessagePack_v1** |    **172.35 ns** |    **NA** |     **24 B** | **0.0036** |      **-** |      **48 B** |
|                 PrivilegeSerialize | MessagePack_v2 |    230.88 ns |    NA |     24 B | 0.0036 |      - |      48 B |
|                 PrivilegeSerialize | MsgPack_v2_opt |    210.68 ns |    NA |     24 B | 0.0036 |      - |      48 B |
|                 PrivilegeSerialize |       SpanJson |    216.68 ns |    NA |     80 B | 0.0079 |      - |     104 B |
|                 PrivilegeSerialize | SystemTextJson |    395.19 ns |    NA |     80 B | 0.0200 |      - |     264 B |
|                **QuestionDeserialize** | **MessagePack_v1** | **11,470.37 ns** |    **NA** |        **-** | **0.5341** | **0.0153** |   **7,032 B** |
|                QuestionDeserialize | MessagePack_v2 | 12,971.23 ns |    NA |        - | 0.5341 | 0.0153 |   7,032 B |
|                QuestionDeserialize | MsgPack_v2_opt | 15,151.03 ns |    NA |        - | 0.5341 | 0.0153 |   7,032 B |
|                QuestionDeserialize |       SpanJson | 18,315.14 ns |    NA |        - | 0.5493 |      - |   7,344 B |
|                QuestionDeserialize | SystemTextJson | 47,765.30 ns |    NA |        - | 0.6104 |      - |   8,544 B |
|                  **QuestionSerialize** | **MessagePack_v1** |  **9,798.00 ns** |    **NA** |   **1646 B** | **0.1221** |      **-** |   **1,672 B** |
|                  QuestionSerialize | MessagePack_v2 |  9,715.97 ns |    NA |   1646 B | 0.1221 |      - |   1,672 B |
|                  QuestionSerialize | MsgPack_v2_opt |  9,433.17 ns |    NA |   1502 B | 0.1068 |      - |   1,528 B |
|                  QuestionSerialize |       SpanJson | 12,030.79 ns |    NA |   6273 B | 0.4730 |      - |   6,288 B |
|                  QuestionSerialize | SystemTextJson | 27,433.77 ns |    NA |   6120 B | 0.5493 |      - |   7,296 B |
|        **QuestionTimelineDeserialize** | **MessagePack_v1** |  **1,131.04 ns** |    **NA** |        **-** | **0.0477** |      **-** |     **624 B** |
|        QuestionTimelineDeserialize | MessagePack_v2 |  1,473.71 ns |    NA |        - | 0.0477 |      - |     624 B |
|        QuestionTimelineDeserialize | MsgPack_v2_opt |  1,498.21 ns |    NA |        - | 0.0477 |      - |     624 B |
|        QuestionTimelineDeserialize |       SpanJson |  1,718.52 ns |    NA |        - | 0.0477 |      - |     624 B |
|        QuestionTimelineDeserialize | SystemTextJson |  5,632.87 ns |    NA |        - | 0.0763 |      - |   1,032 B |
|          **QuestionTimelineSerialize** | **MessagePack_v1** |  **1,023.09 ns** |    **NA** |    **172 B** | **0.0153** |      **-** |     **200 B** |
|          QuestionTimelineSerialize | MessagePack_v2 |  1,036.04 ns |    NA |    172 B | 0.0153 |      - |     200 B |
|          QuestionTimelineSerialize | MsgPack_v2_opt |  1,021.60 ns |    NA |    166 B | 0.0134 |      - |     192 B |
|          QuestionTimelineSerialize |       SpanJson |           NA |    NA |    725 B |      - |      - |         - |
|          QuestionTimelineSerialize | SystemTextJson |  2,926.21 ns |    NA |    695 B | 0.0916 |      - |   1,216 B |
|             **RelatedSiteDeserialize** | **MessagePack_v1** |    **258.76 ns** |    **NA** |        **-** | **0.0124** |      **-** |     **168 B** |
|             RelatedSiteDeserialize | MessagePack_v2 |    344.07 ns |    NA |        - | 0.0124 |      - |     168 B |
|             RelatedSiteDeserialize | MsgPack_v2_opt |    323.14 ns |    NA |        - | 0.0124 |      - |     168 B |
|             RelatedSiteDeserialize |       SpanJson |    336.46 ns |    NA |        - | 0.0124 |      - |     168 B |
|             RelatedSiteDeserialize | SystemTextJson |    701.36 ns |    NA |        - | 0.0124 |      - |     168 B |
|               **RelatedSiteSerialize** | **MessagePack_v1** |    **216.59 ns** |    **NA** |     **29 B** | **0.0041** |      **-** |      **56 B** |
|               RelatedSiteSerialize | MessagePack_v2 |    261.45 ns |    NA |     29 B | 0.0038 |      - |      56 B |
|               RelatedSiteSerialize | MsgPack_v2_opt |    266.18 ns |    NA |     29 B | 0.0038 |      - |      56 B |
|               RelatedSiteSerialize |       SpanJson |    280.83 ns |    NA |     91 B | 0.0091 |      - |     120 B |
|               RelatedSiteSerialize | SystemTextJson |    422.73 ns |    NA |     86 B | 0.0200 |      - |     264 B |
|              **ReputationDeserialize** | **MessagePack_v1** |    **340.79 ns** |    **NA** |        **-** | **0.0124** |      **-** |     **168 B** |
|              ReputationDeserialize | MessagePack_v2 |    464.64 ns |    NA |        - | 0.0124 |      - |     168 B |
|              ReputationDeserialize | MsgPack_v2_opt |    519.11 ns |    NA |        - | 0.0124 |      - |     168 B |
|              ReputationDeserialize |       SpanJson |    541.33 ns |    NA |        - | 0.0124 |      - |     168 B |
|              ReputationDeserialize | SystemTextJson |  1,505.80 ns |    NA |        - | 0.0114 |      - |     168 B |
|       **ReputationHistoryDeserialize** | **MessagePack_v1** |    **177.88 ns** |    **NA** |        **-** | **0.0048** |      **-** |      **64 B** |
|       ReputationHistoryDeserialize | MessagePack_v2 |    283.57 ns |    NA |        - | 0.0048 |      - |      64 B |
|       ReputationHistoryDeserialize | MsgPack_v2_opt |    299.14 ns |    NA |        - | 0.0048 |      - |      64 B |
|       ReputationHistoryDeserialize |       SpanJson |    394.13 ns |    NA |        - | 0.0048 |      - |      64 B |
|       ReputationHistoryDeserialize | SystemTextJson |    950.19 ns |    NA |        - | 0.0038 |      - |      64 B |
|         **ReputationHistorySerialize** | **MessagePack_v1** |    **221.24 ns** |    **NA** |     **32 B** | **0.0041** |      **-** |      **56 B** |
|         ReputationHistorySerialize | MessagePack_v2 |    252.10 ns |    NA |     32 B | 0.0038 |      - |      56 B |
|         ReputationHistorySerialize | MsgPack_v2_opt |    248.06 ns |    NA |     26 B | 0.0038 |      - |      56 B |
|         ReputationHistorySerialize |       SpanJson |    253.78 ns |    NA |    165 B | 0.0143 |      - |     192 B |
|         ReputationHistorySerialize | SystemTextJson |    567.78 ns |    NA |    151 B | 0.0248 |      - |     328 B |
|                **ReputationSerialize** | **MessagePack_v1** |    **341.24 ns** |    **NA** |     **51 B** | **0.0057** |      **-** |      **80 B** |
|                ReputationSerialize | MessagePack_v2 |    385.06 ns |    NA |     51 B | 0.0057 |      - |      80 B |
|                ReputationSerialize | MsgPack_v2_opt |    379.65 ns |    NA |     45 B | 0.0052 |      - |      72 B |
|                ReputationSerialize |       SpanJson |    404.72 ns |    NA |    204 B | 0.0176 |      - |     232 B |
|                ReputationSerialize | SystemTextJson |    762.05 ns |    NA |    180 B | 0.0267 |      - |     360 B |
|                **RevisionDeserialize** | **MessagePack_v1** |  **1,255.76 ns** |    **NA** |        **-** | **0.0629** |      **-** |     **840 B** |
|                RevisionDeserialize | MessagePack_v2 |  1,538.38 ns |    NA |        - | 0.0629 |      - |     840 B |
|                RevisionDeserialize | MsgPack_v2_opt |  1,518.91 ns |    NA |        - | 0.0629 |      - |     840 B |
|                RevisionDeserialize |       SpanJson |  1,731.08 ns |    NA |        - | 0.0668 |      - |     888 B |
|                RevisionDeserialize | SystemTextJson |  5,191.14 ns |    NA |        - | 0.0916 |      - |   1,296 B |
|                  **RevisionSerialize** | **MessagePack_v1** |  **1,129.53 ns** |    **NA** |    **166 B** | **0.0134** |      **-** |     **192 B** |
|                  RevisionSerialize | MessagePack_v2 |  1,374.11 ns |    NA |    166 B | 0.0134 |      - |     192 B |
|                  RevisionSerialize | MsgPack_v2_opt |  1,075.18 ns |    NA |    160 B | 0.0134 |      - |     184 B |
|                  RevisionSerialize |       SpanJson |  1,145.08 ns |    NA |    623 B | 0.0477 |      - |     640 B |
|                  RevisionSerialize | SystemTextJson |  2,769.83 ns |    NA |    592 B | 0.0839 |      - |   1,112 B |
|           **SearchExcerptDeserialize** | **MessagePack_v1** |  **1,586.27 ns** |    **NA** |        **-** | **0.0687** |      **-** |     **904 B** |
|           SearchExcerptDeserialize | MessagePack_v2 |  2,263.28 ns |    NA |        - | 0.0687 |      - |     904 B |
|           SearchExcerptDeserialize | MsgPack_v2_opt |  1,988.06 ns |    NA |        - | 0.0687 |      - |     904 B |
|           SearchExcerptDeserialize |       SpanJson |  2,578.13 ns |    NA |        - | 0.0687 |      - |     928 B |
|           SearchExcerptDeserialize | SystemTextJson |  7,315.91 ns |    NA |        - | 0.0992 |      - |   1,336 B |
|             **SearchExcerptSerialize** | **MessagePack_v1** |  **1,477.46 ns** |    **NA** |    **260 B** | **0.0210** |      **-** |     **288 B** |
|             SearchExcerptSerialize | MessagePack_v2 |  1,456.36 ns |    NA |    260 B | 0.0210 |      - |     288 B |
|             SearchExcerptSerialize | MsgPack_v2_opt |  1,451.21 ns |    NA |    230 B | 0.0191 |      - |     256 B |
|             SearchExcerptSerialize |       SpanJson |  1,764.22 ns |    NA |   1001 B | 0.0763 |      - |   1,016 B |
|             SearchExcerptSerialize | SystemTextJson |  3,905.51 ns |    NA |    966 B | 0.1068 |      - |   1,488 B |
|             **ShallowUserDeserialize** | **MessagePack_v1** |    **433.08 ns** |    **NA** |        **-** | **0.0181** |      **-** |     **240 B** |
|             ShallowUserDeserialize | MessagePack_v2 |    608.22 ns |    NA |        - | 0.0181 |      - |     240 B |
|             ShallowUserDeserialize | MsgPack_v2_opt |    604.34 ns |    NA |        - | 0.0181 |      - |     240 B |
|             ShallowUserDeserialize |       SpanJson |    580.27 ns |    NA |        - | 0.0181 |      - |     240 B |
|             ShallowUserDeserialize | SystemTextJson |  1,756.50 ns |    NA |        - | 0.0477 |      - |     648 B |
|               **ShallowUserSerialize** | **MessagePack_v1** |    **398.61 ns** |    **NA** |     **60 B** | **0.0067** |      **-** |      **88 B** |
|               ShallowUserSerialize | MessagePack_v2 |    421.67 ns |    NA |     60 B | 0.0067 |      - |      88 B |
|               ShallowUserSerialize | MsgPack_v2_opt |    419.58 ns |    NA |     60 B | 0.0067 |      - |      88 B |
|               ShallowUserSerialize |       SpanJson |    439.94 ns |    NA |    239 B | 0.0200 |      - |     264 B |
|               ShallowUserSerialize | SystemTextJson |  1,036.73 ns |    NA |    228 B | 0.0572 |      - |     752 B |
|                    **SiteDeserialize** | **MessagePack_v1** |  **1,585.51 ns** |    **NA** |        **-** | **0.0896** |      **-** |   **1,184 B** |
|                    SiteDeserialize | MessagePack_v2 |  1,787.32 ns |    NA |        - | 0.0896 |      - |   1,184 B |
|                    SiteDeserialize | MsgPack_v2_opt |  1,983.33 ns |    NA |        - | 0.0877 |      - |   1,184 B |
|                    SiteDeserialize |       SpanJson |  2,209.63 ns |    NA |        - | 0.0954 |      - |   1,256 B |
|                    SiteDeserialize | SystemTextJson |  5,010.44 ns |    NA |        - | 0.1221 |      - |   1,664 B |
|                      **SiteSerialize** | **MessagePack_v1** |  **1,249.33 ns** |    **NA** |    **217 B** | **0.0172** |      **-** |     **248 B** |
|                      SiteSerialize | MessagePack_v2 |  1,293.80 ns |    NA |    217 B | 0.0172 |      - |     248 B |
|                      SiteSerialize | MsgPack_v2_opt |  1,278.41 ns |    NA |    199 B | 0.0153 |      - |     224 B |
|                      SiteSerialize |       SpanJson |  1,502.80 ns |    NA |    697 B | 0.0553 |      - |     728 B |
|                      SiteSerialize | SystemTextJson |  2,683.05 ns |    NA |    685 B | 0.0916 |      - |   1,208 B |
|                 **StylingDeserialize** | **MessagePack_v1** |    **228.04 ns** |    **NA** |        **-** | **0.0122** |      **-** |     **160 B** |
|                 StylingDeserialize | MessagePack_v2 |    287.93 ns |    NA |        - | 0.0119 |      - |     160 B |
|                 StylingDeserialize | MsgPack_v2_opt |    270.41 ns |    NA |        - | 0.0119 |      - |     160 B |
|                 StylingDeserialize |       SpanJson |    243.75 ns |    NA |        - | 0.0119 |      - |     160 B |
|                 StylingDeserialize | SystemTextJson |    700.25 ns |    NA |        - | 0.0114 |      - |     160 B |
|                   **StylingSerialize** | **MessagePack_v1** |    **192.67 ns** |    **NA** |     **28 B** | **0.0041** |      **-** |      **56 B** |
|                   StylingSerialize | MessagePack_v2 |    230.92 ns |    NA |     28 B | 0.0041 |      - |      56 B |
|                   StylingSerialize | MsgPack_v2_opt |    237.02 ns |    NA |     28 B | 0.0038 |      - |      56 B |
|                   StylingSerialize |       SpanJson |    288.96 ns |    NA |     93 B | 0.0091 |      - |     120 B |
|                   StylingSerialize | SystemTextJson |    372.00 ns |    NA |     93 B | 0.0205 |      - |     272 B |
|           **SuggestedEditDeserialize** | **MessagePack_v1** |  **1,010.93 ns** |    **NA** |        **-** | **0.0439** |      **-** |     **592 B** |
|           SuggestedEditDeserialize | MessagePack_v2 |  1,191.39 ns |    NA |        - | 0.0439 |      - |     592 B |
|           SuggestedEditDeserialize | MsgPack_v2_opt |  1,231.03 ns |    NA |        - | 0.0439 |      - |     592 B |
|           SuggestedEditDeserialize |       SpanJson |  1,643.46 ns |    NA |        - | 0.0458 |      - |     616 B |
|           SuggestedEditDeserialize | SystemTextJson |  4,661.88 ns |    NA |        - | 0.0763 |      - |   1,024 B |
|             **SuggestedEditSerialize** | **MessagePack_v1** |    **911.88 ns** |    **NA** |    **154 B** | **0.0134** |      **-** |     **184 B** |
|             SuggestedEditSerialize | MessagePack_v2 |    930.57 ns |    NA |    154 B | 0.0134 |      - |     184 B |
|             SuggestedEditSerialize | MsgPack_v2_opt |    915.33 ns |    NA |    136 B | 0.0114 |      - |     160 B |
|             SuggestedEditSerialize |       SpanJson |  1,038.99 ns |    NA |    559 B | 0.0439 |      - |     584 B |
|             SuggestedEditSerialize | SystemTextJson |  2,236.48 ns |    NA |    537 B | 0.0801 |      - |   1,064 B |
|                     **TagDeserialize** | **MessagePack_v1** |    **400.53 ns** |    **NA** |        **-** | **0.0176** |      **-** |     **232 B** |
|                     TagDeserialize | MessagePack_v2 |    497.75 ns |    NA |        - | 0.0172 |      - |     232 B |
|                     TagDeserialize | MsgPack_v2_opt |    488.00 ns |    NA |        - | 0.0172 |      - |     232 B |
|                     TagDeserialize |       SpanJson |    581.30 ns |    NA |        - | 0.0191 |      - |     256 B |
|                     TagDeserialize | SystemTextJson |  1,503.84 ns |    NA |        - | 0.0496 |      - |     664 B |
|                **TagScoreDeserialize** | **MessagePack_v1** |    **631.59 ns** |    **NA** |        **-** | **0.0210** |      **-** |     **280 B** |
|                TagScoreDeserialize | MessagePack_v2 |    736.57 ns |    NA |        - | 0.0210 |      - |     280 B |
|                TagScoreDeserialize | MsgPack_v2_opt |    666.28 ns |    NA |        - | 0.0210 |      - |     280 B |
|                TagScoreDeserialize |       SpanJson |    816.60 ns |    NA |        - | 0.0210 |      - |     280 B |
|                TagScoreDeserialize | SystemTextJson |  2,506.05 ns |    NA |        - | 0.0496 |      - |     688 B |
|                  **TagScoreSerialize** | **MessagePack_v1** |    **449.00 ns** |    **NA** |     **71 B** | **0.0067** |      **-** |      **96 B** |
|                  TagScoreSerialize | MessagePack_v2 |    496.70 ns |    NA |     71 B | 0.0067 |      - |      96 B |
|                  TagScoreSerialize | MsgPack_v2_opt |    497.18 ns |    NA |     71 B | 0.0067 |      - |      96 B |
|                  TagScoreSerialize |       SpanJson |    521.63 ns |    NA |    292 B | 0.0238 |      - |     320 B |
|                  TagScoreSerialize | SystemTextJson |  1,395.42 ns |    NA |    281 B | 0.0610 |      - |     800 B |
|                       **TagSerialize** | **MessagePack_v1** |    **357.81 ns** |    **NA** |     **48 B** | **0.0052** |      **-** |      **72 B** |
|                       TagSerialize | MessagePack_v2 |    419.12 ns |    NA |     48 B | 0.0052 |      - |      72 B |
|                       TagSerialize | MsgPack_v2_opt |    385.54 ns |    NA |     42 B | 0.0052 |      - |      72 B |
|                       TagSerialize |       SpanJson |    375.31 ns |    NA |    201 B | 0.0176 |      - |     232 B |
|                       TagSerialize | SystemTextJson |    983.94 ns |    NA |    201 B | 0.0534 |      - |     720 B |
|              **TagSynonymDeserialize** | **MessagePack_v1** |    **317.76 ns** |    **NA** |        **-** | **0.0114** |      **-** |     **152 B** |
|              TagSynonymDeserialize | MessagePack_v2 |    480.39 ns |    NA |        - | 0.0114 |      - |     152 B |
|              TagSynonymDeserialize | MsgPack_v2_opt |    381.01 ns |    NA |        - | 0.0114 |      - |     152 B |
|              TagSynonymDeserialize |       SpanJson |    511.52 ns |    NA |        - | 0.0114 |      - |     152 B |
|              TagSynonymDeserialize | SystemTextJson |  1,039.42 ns |    NA |        - | 0.0114 |      - |     152 B |
|                **TagSynonymSerialize** | **MessagePack_v1** |    **319.18 ns** |    **NA** |     **54 B** | **0.0057** |      **-** |      **80 B** |
|                TagSynonymSerialize | MessagePack_v2 |    336.03 ns |    NA |     54 B | 0.0057 |      - |      80 B |
|                TagSynonymSerialize | MsgPack_v2_opt |    321.36 ns |    NA |     42 B | 0.0052 |      - |      72 B |
|                TagSynonymSerialize |       SpanJson |    406.09 ns |    NA |    172 B | 0.0153 |      - |     200 B |
|                TagSynonymSerialize | SystemTextJson |    671.23 ns |    NA |    172 B | 0.0267 |      - |     352 B |
|                 **TagWikiDeserialize** | **MessagePack_v1** |  **1,166.28 ns** |    **NA** |        **-** | **0.0515** |      **-** |     **688 B** |
|                 TagWikiDeserialize | MessagePack_v2 |  1,616.12 ns |    NA |        - | 0.0515 |      - |     688 B |
|                 TagWikiDeserialize | MsgPack_v2_opt |  1,478.44 ns |    NA |        - | 0.0515 |      - |     688 B |
|                 TagWikiDeserialize |       SpanJson |  1,757.04 ns |    NA |        - | 0.0515 |      - |     688 B |
|                 TagWikiDeserialize | SystemTextJson |  4,988.76 ns |    NA |        - | 0.0763 |      - |   1,096 B |
|                   **TagWikiSerialize** | **MessagePack_v1** |  **1,006.73 ns** |    **NA** |    **178 B** | **0.0153** |      **-** |     **208 B** |
|                   TagWikiSerialize | MessagePack_v2 |  1,032.34 ns |    NA |    178 B | 0.0153 |      - |     208 B |
|                   TagWikiSerialize | MsgPack_v2_opt |  1,030.26 ns |    NA |    166 B | 0.0134 |      - |     192 B |
|                   TagWikiSerialize |       SpanJson |  1,428.22 ns |    NA |    695 B | 0.0553 |      - |     728 B |
|                   TagWikiSerialize | SystemTextJson |  2,787.02 ns |    NA |    674 B | 0.0877 |      - |   1,192 B |
|                  **TopTagDeserialize** | **MessagePack_v1** |    **249.07 ns** |    **NA** |        **-** | **0.0079** |      **-** |     **104 B** |
|                  TopTagDeserialize | MessagePack_v2 |    303.42 ns |    NA |        - | 0.0076 |      - |     104 B |
|                  TopTagDeserialize | MsgPack_v2_opt |    312.59 ns |    NA |        - | 0.0076 |      - |     104 B |
|                  TopTagDeserialize |       SpanJson |    342.14 ns |    NA |        - | 0.0076 |      - |     104 B |
|                  TopTagDeserialize | SystemTextJson |  1,083.41 ns |    NA |        - | 0.0076 |      - |     104 B |
|                    **TopTagSerialize** | **MessagePack_v1** |    **240.22 ns** |    **NA** |     **35 B** | **0.0048** |      **-** |      **64 B** |
|                    TopTagSerialize | MessagePack_v2 |    259.14 ns |    NA |     35 B | 0.0048 |      - |      64 B |
|                    TopTagSerialize | MsgPack_v2_opt |    251.28 ns |    NA |     35 B | 0.0048 |      - |      64 B |
|                    TopTagSerialize |       SpanJson |    252.12 ns |    NA |    151 B | 0.0134 |      - |     176 B |
|                    TopTagSerialize | SystemTextJson |    585.27 ns |    NA |    151 B | 0.0248 |      - |     328 B |
|                    **UserDeserialize** | **MessagePack_v1** |  **1,097.46 ns** |    **NA** |        **-** | **0.0420** |      **-** |     **552 B** |
|                    UserDeserialize | MessagePack_v2 |  1,374.25 ns |    NA |        - | 0.0420 |      - |     552 B |
|                    UserDeserialize | MsgPack_v2_opt |  1,407.90 ns |    NA |        - | 0.0420 |      - |     552 B |
|                    UserDeserialize |       SpanJson |  2,062.15 ns |    NA |        - | 0.0420 |      - |     552 B |
|                    UserDeserialize | SystemTextJson |  5,219.30 ns |    NA |        - | 0.0687 |      - |     960 B |
|                      **UserSerialize** | **MessagePack_v1** |  **1,097.21 ns** |    **NA** |    **210 B** | **0.0172** |      **-** |     **240 B** |
|                      UserSerialize | MessagePack_v2 |  1,078.78 ns |    NA |    210 B | 0.0172 |      - |     240 B |
|                      UserSerialize | MsgPack_v2_opt |  1,038.71 ns |    NA |    186 B | 0.0153 |      - |     216 B |
|                      UserSerialize |       SpanJson |  1,543.71 ns |    NA |    882 B | 0.0687 |      - |     912 B |
|                      UserSerialize | SystemTextJson |  2,770.40 ns |    NA |    872 B | 0.1068 |      - |   1,400 B |
|            **UserTimelineDeserialize** | **MessagePack_v1** |    **439.15 ns** |    **NA** |        **-** | **0.0176** |      **-** |     **232 B** |
|            UserTimelineDeserialize | MessagePack_v2 |    620.11 ns |    NA |        - | 0.0172 |      - |     232 B |
|            UserTimelineDeserialize | MsgPack_v2_opt |    625.17 ns |    NA |        - | 0.0172 |      - |     232 B |
|            UserTimelineDeserialize |       SpanJson |    694.56 ns |    NA |        - | 0.0172 |      - |     232 B |
|            UserTimelineDeserialize | SystemTextJson |  1,730.19 ns |    NA |        - | 0.0172 |      - |     232 B |
|              **UserTimelineSerialize** | **MessagePack_v1** |    **515.69 ns** |    **NA** |     **70 B** | **0.0072** |      **-** |      **96 B** |
|              UserTimelineSerialize | MessagePack_v2 |    502.96 ns |    NA |     70 B | 0.0067 |      - |      96 B |
|              UserTimelineSerialize | MsgPack_v2_opt |    473.59 ns |    NA |     64 B | 0.0067 |      - |      88 B |
|              UserTimelineSerialize |       SpanJson |           NA |    NA |    274 B |      - |      - |         - |
|              UserTimelineSerialize | SystemTextJson |  1,006.56 ns |    NA |    257 B | 0.0324 |      - |     432 B |
|         **WritePermissionDeserialize** | **MessagePack_v1** |    **312.31 ns** |    **NA** |        **-** | **0.0081** |      **-** |     **112 B** |
|         WritePermissionDeserialize | MessagePack_v2 |    321.16 ns |    NA |        - | 0.0081 |      - |     112 B |
|         WritePermissionDeserialize | MsgPack_v2_opt |    341.00 ns |    NA |        - | 0.0081 |      - |     112 B |
|         WritePermissionDeserialize |       SpanJson |    334.11 ns |    NA |        - | 0.0081 |      - |     112 B |
|         WritePermissionDeserialize | SystemTextJson |  1,089.76 ns |    NA |        - | 0.0076 |      - |     112 B |
|           **WritePermissionSerialize** | **MessagePack_v1** |    **240.76 ns** |    **NA** |     **28 B** | **0.0038** |      **-** |      **56 B** |
|           WritePermissionSerialize | MessagePack_v2 |    253.15 ns |    NA |     28 B | 0.0038 |      - |      56 B |
|           WritePermissionSerialize | MsgPack_v2_opt |    275.03 ns |    NA |     28 B | 0.0038 |      - |      56 B |
|           WritePermissionSerialize |       SpanJson |    230.85 ns |    NA |    168 B | 0.0153 |      - |     200 B |
|           WritePermissionSerialize | SystemTextJson |    598.04 ns |    NA |    168 B | 0.0257 |      - |     344 B |

Benchmarks with issues:
  AllSerializerBenchmark_BytesInOut._PrimitiveGuidDeserialize: ShortRun(Jit=RyuJit, Platform=X64, Runtime=.NET 6.0, IterationCount=1, LaunchCount=1, WarmupCount=1) [Serializer=SpanJson]
  AllSerializerBenchmark_BytesInOut.ClosedDetailsDeserialize: ShortRun(Jit=RyuJit, Platform=X64, Runtime=.NET 6.0, IterationCount=1, LaunchCount=1, WarmupCount=1) [Serializer=SpanJson]
  AllSerializerBenchmark_BytesInOut.FlagOptionDeserialize: ShortRun(Jit=RyuJit, Platform=X64, Runtime=.NET 6.0, IterationCount=1, LaunchCount=1, WarmupCount=1) [Serializer=SpanJson]
  AllSerializerBenchmark_BytesInOut.MigrationInfoSerialize: ShortRun(Jit=RyuJit, Platform=X64, Runtime=.NET 6.0, IterationCount=1, LaunchCount=1, WarmupCount=1) [Serializer=SpanJson]
  AllSerializerBenchmark_BytesInOut.QuestionTimelineSerialize: ShortRun(Jit=RyuJit, Platform=X64, Runtime=.NET 6.0, IterationCount=1, LaunchCount=1, WarmupCount=1) [Serializer=SpanJson]
  AllSerializerBenchmark_BytesInOut.UserTimelineSerialize: ShortRun(Jit=RyuJit, Platform=X64, Runtime=.NET 6.0, IterationCount=1, LaunchCount=1, WarmupCount=1) [Serializer=SpanJson]
