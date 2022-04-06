RandomTreeTest(10, 10000, 9990, 0, 99, false, false, false);

List<int> ins = new() { 78, 50, 2, 20, 68, 19, 39 };
List<int> del = new() { 20, 2, 39 };
CaseTest(ins, del);

InteractiveTree();

static void CaseTest(List<int> ins, List<int> del)
{
    RedBlackTree tree = new();

    ins.ForEach(i => tree.Insert(new Node(i)));
    RedBlackTreePrinter.PrintNode(tree.Root);
    del.ForEach(i =>
    {
        tree.Delete(tree.Search(i));
        RedBlackTreePrinter.PrintNode(tree.Root);

    });
}

static void Shuffle<T>(IList<T> list)
{
    var r = new Random();
    for (var i = 0; i < list.Count; i++)
    {
        var j = r.Next(list.Count);
        (list[i], list[j]) = (list[j], list[i]);
    }
}

static void InteractiveTree()
{
    RedBlackTree t = new();
    while (true)
    {
        try
        {
            Console.WriteLine("Operace i, d, p, in, pre, q:");
            switch (Console.ReadLine()?.ToLower())
            {
                case "i":
                    Console.WriteLine("Key: ");
                    t.Insert(new Node(int.Parse(Console.ReadLine() ?? throw new InvalidOperationException())));
                    break;
                case "d":
                    Console.WriteLine("Key: ");
                    t.Delete(t.Search(int.Parse(Console.ReadLine() ?? throw new InvalidOperationException())));
                    break;
                case "p":
                    RedBlackTreePrinter.PrintNode(t.Root);
                    break;
                case "in":
                    RedBlackTree.InOrder(t.Root);
                    break;
                case "pre":
                    RedBlackTree.PreOrder(t.Root);
                    break;
                case "q":
                    return;
            }
        }
        catch
        {
            Console.WriteLine("Invalid input");
        }
    }
}

static void RandomTreeTest(int trees, int insert, int delete, int minKey, int maxKey, bool print, bool result, bool stepper)
{
    for (var j = 0; j < trees; j++)
    {
        var listRands = new List<int>();
        RedBlackTree t = new();

        for (var i = 0; i < insert; i++)
        {
            var rand = Random.Shared.Next(minKey, maxKey);
            listRands.Add(rand);
            if (print) Console.WriteLine(i + ". " + "Insert: " + rand);
            t.Insert(new Node(rand));
            t.ValidationTest();
        }
        if (print)
            RedBlackTreePrinter.PrintNode(t.Root);
        if (stepper)
            Console.ReadLine();
        Shuffle(listRands);

        for (var i = 0; i < delete; i++)
        {
            if (print) Console.WriteLine(i + ". " + "Delete: " + listRands[i]);
            t.Delete(t.Search(listRands[i]));
            t.ValidationTest();
        }

        if (print || result)
            RedBlackTreePrinter.PrintNode(t.Root);
        if (stepper)
            Console.ReadLine();
    }
}

public enum RbColor
{
    Red,
    Black
}

internal class Node
{
    public int Key { get; }
    public Node Left { get; set; }
    public Node Right { get; set; }
    public Node Parent { get; set; }
    public RbColor Color { get; set; }

    public Node(int key)
    {
        Key = key;
        Color = RbColor.Red;
        Left = RedBlackTree.Nil;
        Right = RedBlackTree.Nil;
        Parent = RedBlackTree.Nil;
    }
}

internal class RedBlackTree
{
    public static Node Nil { get; private set; } = null!;

    public Node Root { get; private set; }

    public int Size { get; private set; }

    public RedBlackTree()
    {
        Nil = new Node(int.MinValue)
        {
            Color = RbColor.Black
        };

        Root = Nil;
        Size = 0;
    }

    public Node? Search(int key)
    {
#if DEBUG
        Console.WriteLine("Search: " + key);
#endif        
        var node = Root;
        while (node != Nil)
        {
            if (node.Key == key)
                return node;
            node = node.Key < key ? node.Right : node.Left;
        }
        return null;
    }

    public void Insert(Node? z)
    {
#if DEBUG
        Console.WriteLine("Insert: " + z?.Key);
#endif
        if (z == null) return;
        Size++;

        var y = Nil;
        var x = this.Root;

        while (x != Nil)
        {
            y = x;
            x = z.Key < x.Key ? x.Left : x.Right;
        }

        z.Parent = y;

        if (y == Nil)
        {
            Root = z;
        }
        else if (z.Key < y.Key)
        {
            y.Left = z;
        }
        else
        {
            y.Right = z;
        }

        z.Left = Nil;
        z.Right = Nil;
        z.Color = RbColor.Red;

        InsertFixup(z);
    }

    private void InsertFixup(Node z)
    {
#if DEBUG
        Console.WriteLine("InsertFixup: " + z.Key);
#endif
        while (z.Parent.Color == RbColor.Red)
        {
            if (z.Parent == z.Parent.Parent.Left)
            {
                var y = z.Parent.Parent.Right;

                if (y.Color == RbColor.Red)
                {
                    z.Parent.Color = RbColor.Black;
                    y.Color = RbColor.Black;
                    z.Parent.Parent.Color = RbColor.Red;
                    z = z.Parent.Parent;
                }
                else
                {
                    if (z == z.Parent.Right)
                    {
                        z = z.Parent;
                        LeftRotate(z);
                    }

                    z.Parent.Color = RbColor.Black;
                    z.Parent.Parent.Color = RbColor.Red;
                    RightRotate(z.Parent.Parent);
                }
            }
            else
            {
                var y = z.Parent.Parent.Left;

                if (y.Color == RbColor.Red)
                {
                    z.Parent.Color = RbColor.Black;
                    y.Color = RbColor.Black;
                    z.Parent.Parent.Color = RbColor.Red;
                    z = z.Parent.Parent;
                }
                else
                {
                    if (z == z.Parent.Left)
                    {
                        z = z.Parent;
                        RightRotate(z);
                    }

                    z.Parent.Color = RbColor.Black;
                    z.Parent.Parent.Color = RbColor.Red;
                    LeftRotate(z.Parent.Parent);
                }
            }
        }
        Root.Color = RbColor.Black;
    }

    public void Delete(Node? z)
    {
#if DEBUG
        Console.WriteLine("Delete: " + z?.Key);
#endif
        if (z == null || z == Nil)
        {
            throw new Exception();
        }
        Size--;

        var y = z;
        var yOriginalColor = y.Color;
        Node x;

        if (z.Left == Nil)
        {
            x = z.Right;
            Transplant(z, z.Right);
        }
        else if (z.Right == Nil)
        {
            x = z.Left;
            Transplant(z, z.Left);
        }
        else
        {
            y = Minimum(z.Right);
            yOriginalColor = y.Color;
            x = y.Right;
            if (y.Parent == z)
            {
                x.Parent = y;
            }
            else
            {
                Transplant(y, y.Right);
                y.Right = z.Right;
                y.Right.Parent = y;
            }
            Transplant(z, y);
            y.Left = z.Left;
            y.Left.Parent = y;
            y.Color = z.Color;
        }

        if (yOriginalColor == RbColor.Black)
            DeleteFixup(x);
    }

    private void DeleteFixup(Node x)
    {
#if DEBUG
        Console.WriteLine("DeleteFixup: " + x.Key);
#endif

        while (x != Root && x.Color == RbColor.Black)
        {
            if (x == x.Parent.Left)
            {
                var w = x.Parent.Right;

                if (w.Color == RbColor.Red)
                {
                    w.Color = RbColor.Black;
                    x.Parent.Color = RbColor.Red;
                    LeftRotate(x.Parent);
                    w = x.Parent.Right;
                }
                if (w.Left.Color == RbColor.Black && w.Right.Color == RbColor.Black)
                {
                    w.Color = RbColor.Red;
                    x = x.Parent;
                }
                else
                {
                    if (w.Right.Color == RbColor.Black)
                    {
                        w.Left.Color = RbColor.Black;
                        w.Color = RbColor.Red;
                        RightRotate(w);
                        w = x.Parent.Right;
                    }
                    w.Color = x.Parent.Color;
                    x.Parent.Color = RbColor.Black;
                    w.Right.Color = RbColor.Black;
                    LeftRotate(x.Parent);
                    x = Root;
                }
            }
            else
            {
                var w = x.Parent.Left;

                if (w.Color == RbColor.Red)
                {
                    w.Color = RbColor.Black;
                    x.Parent.Color = RbColor.Red;
                    RightRotate(x.Parent);
                    w = x.Parent.Left;
                }
                if (w.Right.Color == RbColor.Black && w.Left.Color == RbColor.Black)
                {
                    w.Color = RbColor.Red;
                    x = x.Parent;
                }
                else
                {
                    if (w.Left.Color == RbColor.Black)
                    {
                        w.Right.Color = RbColor.Black;
                        w.Color = RbColor.Red;
                        LeftRotate(w);
                        w = x.Parent.Left;
                    }
                    w.Color = x.Parent.Color;
                    x.Parent.Color = RbColor.Black;
                    w.Left.Color = RbColor.Black;
                    RightRotate(x.Parent);
                    x = Root;
                }
            }
        }
        x.Color = RbColor.Black;
    }

    private static Node Minimum(Node node)
    {
#if DEBUG
        Console.WriteLine("Minimum: " + node.Key);
#endif
        while (true)
        {
            if (node.Left == Nil) return node;
            node = node.Left;
        }
    }

    private void Transplant(Node u, Node v)
    {
#if DEBUG
        Console.WriteLine("Transplant: " + u.Key + " | " + v.Key);
#endif
        if (u.Parent == Nil)
        {
            Root = v;
        }
        else if (u == u.Parent.Left)
        {
            u.Parent.Left = v;
        }
        else
        {
            u.Parent.Right = v;
        }
        v.Parent = u.Parent;
    }

    private void LeftRotate(Node x)
    {
#if DEBUG
        Console.WriteLine("LeftRotate: " + x.Key);
#endif
        if (x.Right == Nil || Root.Parent != Nil)
        {
            throw new Exception("LeftRotate: x.Right == Nil || Root.Parent != Nil");
        }

        var y = x.Right;
        x.Right = y.Left;
        if (y.Left != Nil)
        {
            y.Left.Parent = x;
        }
        y.Parent = x.Parent;
        if (x.Parent == Nil)
        {
            Root = y;
        }
        else if (x == x.Parent.Left)
        {
            x.Parent.Left = y;
        }
        else
        {
            x.Parent.Right = y;
        }

        y.Left = x;
        x.Parent = y;
    }

    private void RightRotate(Node x)
    {
#if DEBUG
        Console.WriteLine("RightRotate: " + x.Key);
#endif
        if (x.Left == Nil || Root.Parent != Nil)
        {
            throw new Exception("RightRotate: x.Left == Nil || Root.Parent != Nil");
        }

        var y = x.Left;
        x.Left = y.Right;

        if (y.Right != Nil)
        {
            y.Right.Parent = x;
        }
        y.Parent = x.Parent;
        if (x.Parent == Nil)
        {
            Root = y;
        }
        else if (x == x.Parent.Right)
        {
            x.Parent.Right = y;
        }
        else
        {
            x.Parent.Left = y;
        }

        y.Right = x;
        x.Parent = y;
    }

    public static void InOrder(Node node)
    {
        while (true)
        {
            if (node == Nil) return;

            InOrder(node.Left);
            Console.ForegroundColor = node.Color == RbColor.Red ? ConsoleColor.Red : ConsoleColor.White;

            Console.Write(node.Key);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(", ");

            node = node.Right;
        }
    }

    public static void PreOrder(Node node)
    {
        while (true)
        {
            if (node == Nil) return;

            Console.ForegroundColor = node.Color == RbColor.Red ? ConsoleColor.Red : ConsoleColor.White;

            Console.Write(node.Key);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(", ");

            PreOrder(node.Left);
            node = node.Right;
        }
    }

    private static int Depth(Node node)
    {
        return node == Nil ? 1 : Math.Max(Depth(node.Left), Depth(node.Right));
    }

    public void ValidationTest()
    {
        //TODO test 5. condition black nodes = all paths to leafs

        if (Root == Nil) return;

        if (Root != Nil && Root.Color == RbColor.Red)
        {
            throw new Exception("Root is red");
        }

        if (Depth(Root) - 1 > 2 * Math.Ceiling(Math.Log2((Size + 1))))
        {
            throw new Exception("Tree is too high");
        }

        if (Nil.Color == RbColor.Red) throw new Exception("Nil is red!");

        ValidationTest(Root);
    }

    private static void ValidationTest(Node node)
    {
        if (node == Nil)
            return;

        if (node.Color == RbColor.Red && (node.Left.Color == RbColor.Red || node.Right.Color == RbColor.Red))
        {
            throw new Exception("Red node has red child");
        }

        ValidationTest(node.Left);
        ValidationTest(node.Right);
    }

}

//RedBlackTreePrinter, ukraden od spolužáka. Ten to ukradl nevím odkud.
internal static class RedBlackTreePrinter
{
    public static void PrintNode(Node root)
    {
        var maxLevel = MaxLevel(root);

        PrintNodeInternal(new List<Node?>() { root }, 1, maxLevel);
    }

    private static void PrintNodeInternal(List<Node?> nodes, int level, int maxLevel)
    {
        while (true)
        {
            if (nodes.Count == 0 || IsAllElementsNull(nodes)) return;

            var floor = maxLevel - level;
            var edgeLines = (int)Math.Pow(2, (Math.Max(floor - 1, 0)));
            var firstSpaces = (int)Math.Pow(2, (floor)) - 1;
            var betweenSpaces = (int)Math.Pow(2, (floor + 1)) - 1;

            PrintWhitespaces(firstSpaces);

            List<Node?> newNodes = new();
            foreach (var node in nodes)
            {
                if (node != null && node != RedBlackTree.Nil)
                {
                    Console.BackgroundColor = node.Color == RbColor.Red ? ConsoleColor.DarkRed : ConsoleColor.Black;
                    Console.Write(node.Key);
                    Console.BackgroundColor = ConsoleColor.Black;
                    newNodes.Add(node.Left);
                    newNodes.Add(node.Right);
                }
                else
                {
                    newNodes.Add(null);
                    newNodes.Add(null);
                    Console.Write(" ");
                }

                PrintWhitespaces(betweenSpaces);
            }

            Console.WriteLine("");


            for (var i = 1; i <= edgeLines; i++)
            {
                foreach (var t in nodes)
                {
                    PrintWhitespaces(firstSpaces - i);
                    if (t == null || t == RedBlackTree.Nil)
                    {
                        PrintWhitespaces(edgeLines + edgeLines + i + 1);
                        continue;
                    }

                    if (t.Left != RedBlackTree.Nil)
                        Console.Write("/");
                    else
                        PrintWhitespaces(1);

                    PrintWhitespaces(i + i - 1);

                    if (t.Right != RedBlackTree.Nil)
                        Console.Write("\\");
                    else
                        PrintWhitespaces(1);

                    PrintWhitespaces(edgeLines + edgeLines - i);
                }

                Console.WriteLine("");
            }

            nodes = newNodes;
            level = level + 1;
        }
    }

    private static void PrintWhitespaces(int count)
    {
        for (var i = 0; i < count; i++)
            Console.Write(" ");
    }

    private static int MaxLevel(Node? node)
    {
        if (node == null || node == RedBlackTree.Nil)
            return 0;

        return Math.Max(MaxLevel(node.Left), MaxLevel(node.Right)) + 1;
    }

    private static bool IsAllElementsNull(IEnumerable<Node?> list)
    {
        return list.All(node => node == RedBlackTree.Nil || node == null);
    }
}