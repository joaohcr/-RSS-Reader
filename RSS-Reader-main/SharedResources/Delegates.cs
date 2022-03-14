using System;

namespace SharedResources
{
    public delegate void MethodWithString(string str);

    public delegate void MethodWithoutArg();

    public delegate void MethodWithItem(RSSItem item);

    public delegate void MethodWithUri(Uri uri);

    public delegate RSSItem MethodWithItemReturned(string str);

    public delegate void MethodWithFeed(RSSFeed feed);
}
