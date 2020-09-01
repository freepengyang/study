using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public static class TimeCalculate
{
    static long begin;
    static Stopwatch sw = new Stopwatch();
    public static void Begin()
    {
        sw.Restart();
    }
    public static void End(string fmt,params object[] argv)
    {
        sw.Stop();
        UnityEngine.Debug.LogFormat("[{0}:ms]|[{1}:ws] => [{2}]",sw.ElapsedMilliseconds, sw.ElapsedTicks, string.Format(fmt, argv));
    }
}

public class FastNode
{
    public object value;
    public FastNode next;
    public FastNode prev;
}
//用于元类型数据数组
public class FastLink
{
    static Stack<FastNode> ms_nodes_pool = new Stack<FastNode>(1024);

    static FastNode Get()
    {
        if (ms_nodes_pool.Count > 0)
            return ms_nodes_pool.Pop();
        return new FastNode();
    }

    static void Put(FastNode node)
    {
        node.value = null;
        node.prev = null;
        node.next = null;
        ms_nodes_pool.Push(node);
    }

    Dictionary<long, FastNode> mValue2Nodes;

    public FastLink(int capacity = 8)
    {
        mValue2Nodes = new Dictionary<long, FastNode>(capacity);
    }

    FastNode root;
    FastNode tail;
    public int Count
    {
        get
        {
            return mValue2Nodes.Count;
        }
    }

    public void Travel(System.Action<object> element)
    {
        FastNode node = root;
        while (null != node && node != tail)
        {
            element(node.value);
            node = node.next;
        }
    }

    public void Append(long id,object value)
    {
        if (null == root)
        {
            root = Get();
            root.value = value;
            tail = Get();
            tail.prev = root;
            root.next = tail;
            mValue2Nodes.Add(id, root);
        }
        else
        {
            FastNode prev = tail.prev;
            var next = Get();
            next.value = value;
            next.prev = prev;
            next.next = tail;
            prev.next = next;
            tail.prev = next;
            mValue2Nodes.Add(id, next);
        }
    }

    public void Remove(long id)
    {
        FastNode node = null;
        if(mValue2Nodes.TryGetValue(id, out node))
        {
            mValue2Nodes.Remove(id);
            if (node == root)
            {
                node.next.prev = null;
                root = node.next;
                Put(node);
            }
            else
            {
                FastNode prev = node.prev;
                FastNode next = node.next;
                prev.next = next;
                next.prev = prev;
                Put(node);
            }
        }
    }

    public void Print()
    {
        Travel(value =>
        {
            UnityEngine.Debug.LogFormat("node:[{0}]", value);
        });
    }

    public void Clear()
    {
        mValue2Nodes.Clear();
        while (null != root)
        {
            var next = root.next;
            Put(root);
            root = next;
        }
    }
}