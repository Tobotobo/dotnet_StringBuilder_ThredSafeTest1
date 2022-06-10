# dotnet_StringBuilder_ThredSafeTest1

## 概要
C# の StringBuilder がスレッドセーフではないという話を耳にしたので実験してみました。

## 環境
```
$ ver
Microsoft Windows [Version 10.0.19044.1706]
$ dotnet --version
7.0.100-preview.3.22179.4
```

## 結果
確かにスレッドセーフではありませんでした。  
以下は、3 スレッドで 1,000 文字を 1 文字ずつ追加した後の StringBuilder の Length が 3,000 か確認したものですが、ちょくちょく 3,000 でないことがあります。
```
$ dotnet run
#1 × 2139
#2 × 2971
#3 ○
#4 ○
#5 ○
#6 ○
#7 ○
#8 × 2638
#9 ○
```
また、例外が発生する場合もあります。
```
System.ArgumentException: Destination is too short. (Parameter 'destination')
   at System.Text.StringBuilder.AppendWithExpansion(Char& value, Int32 valueCount)
   at System.Text.StringBuilder.Append(String value)
   at Program.<>c__DisplayClass0_0.<<Main>$>b__1()
```
## 対応
当たり前ですが、lock すれば問題は発生しません。
```cs
lock (sb)
{
    for (int i = 0; i < APPEND_LENGTH; i++)
        sb.Append(s);
}
```
```
$ dotnet run
#1 ○
#2 ○
#3 ○
#4 ○
#5 ○
#6 ○
#7 ○
#8 ○
#9 ○
```