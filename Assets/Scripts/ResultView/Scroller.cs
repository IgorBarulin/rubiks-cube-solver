using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Scroller : UIBehaviour
{
    public CommandView CurrentItem { get { return _items[_curItem]; } }

    private RectTransform _contentRt;
    private CommandView[] _itemStock;

    private List<CommandView> _items = new List<CommandView>();

    private int _curItem;

    protected override void Awake()
    {
        base.Awake();

        ScrollRect scrollRect = gameObject.GetComponentInChildren<ScrollRect>();
        _contentRt = scrollRect.content.transform as RectTransform;
        _itemStock = gameObject.GetComponentsInChildren<CommandView>();
    }

    public void Initialize(string[] combination)
    {
        _items.Clear();
        int i = 0;
        foreach (var item in _itemStock)
        {
            item.gameObject.SetActive(i < combination.Length);
            if (i < combination.Length)
            {
                item.Initialize(combination[i]);
                _items.Add(item);
            }
            i++;
        }

        _curItem = 0;
        _items[_curItem].Backlight = true;
    }

    public bool Next()
    {
        if (_curItem + 1 < _items.Count)
        {
            _items[_curItem].Backlight = false;
            _items[++_curItem].Backlight = true;

            if (_curItem > 2 && _curItem < _items.Count - 2)
            {
                RectTransform itemRect = _items[_curItem].transform as RectTransform;
                _contentRt.anchoredPosition -= new Vector2(itemRect.sizeDelta.x, 0);
            }

            return true;
        }

        return false;
    }

    public bool Prev()
    {
        if (_curItem - 1 > -1)
        {
            _items[_curItem].Backlight = false;
            _items[--_curItem].Backlight = true;

            if (_curItem > 1 && _curItem < _items.Count - 3)
            {
                RectTransform itemRect = _items[_curItem].transform as RectTransform;
                _contentRt.anchoredPosition += new Vector2(itemRect.sizeDelta.x, 0);
            }

            return true;
        }

        return false;
    }
}
