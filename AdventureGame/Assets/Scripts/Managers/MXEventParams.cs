using System;

/// <summary>
/// Custom event parameter to pass any value (classes included)
/// </summary>
/// <typeparam name="T">the type of the value</typeparam>
public class MXEventParams<T>
{
    public T Param { get; set; }
    public MXEventParams(T param) 
   {
        Param = param;
   }

}
