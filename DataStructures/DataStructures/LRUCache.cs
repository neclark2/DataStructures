using System.Collections.Generic;

public class LRUCache<TKey, TValue>
{
    private Dictionary<TKey, LRUNode<TKey, TValue>> _lookUp = new Dictionary<TKey, LRUNode<TKey, TValue>>();
    private List<LRUNode<TKey, TValue>> _list = new List<LRUNode<TKey, TValue>>();

    //this would be set via external configuration
    private const int maxSize = 3;

    public TValue Get(TKey key)
    {
        if (!_lookUp.ContainsKey(key))
        {
            //either the key wasn't added or has since been evicted from the cache
            throw new KeyNotFoundException();
        }

        //Remove and add to end, this is O(N) in the worst case.  I think using a linked list would be O(1), because you wouldn't need to iterate through the list to find where to remove it
        _list.Remove(_lookUp[key]);
        _list.Add(_lookUp[key]);

        return _lookUp[key].Value;
    }

    public void Add(TKey key, TValue value)
    {
        if (_lookUp.ContainsKey(key))
        {
            //if the key already exists, just remove and replace
            Remove(key);
        }

        var node = new LRUNode<TKey, TValue> { Key = key, Value = value };
        _list.Add(node);
        _lookUp.Add(key, node);

        if (_list.Count > maxSize)
        {
            _list.RemoveAt(0);
        }
    }

    public void Remove(TKey key)
    {
        if (!_lookUp.ContainsKey(key))
        {
            throw new KeyNotFoundException();
        }

        //This is O(N) in the worst case.  I think using a linked list would be O(1), because you wouldn't need to iterate through the list to find where to remove it.
        _list.Remove(_lookUp[key]);
        _lookUp.Remove(key);
    }
}

public class LRUNode<TKey, TValue>
{
    public TKey Key { get; set; }
    public TValue Value { get; set; }
}