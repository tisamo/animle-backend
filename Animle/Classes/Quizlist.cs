﻿namespace Animle.Classes
{
    public class ListResponse<T>
    {
        public List<T> List { get; set; }
        public int Count { get; set; }
    }
}