using System;

public class MXEventParams<T>
{
    public T Param { get; set; }
    public MXEventParams(T param) 
   {
        Param = param;
   }

}
