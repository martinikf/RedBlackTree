Random r = new();

for (var j = 0; j < 10; j++)
{
    var listRands = new List<int>();
    RedBlackTree t = new();
    
    for (var i = 0; i < 10; i++)
    {
        var rand = r.Next(100);
        listRands.Add(rand);
        Console.WriteLine(i + ". " + "Insert: " + rand);
        t.Insert(new Node(rand));
        t.ValidationTest();
    }

    listRands = Shuffle(listRands);

    for (var i = 0; i < 10; i++)
    {
        Console.WriteLine(i + ". " + "Delete: " + listRands[i]);
        t.Delete(t.Search(listRands[i]));
        t.ValidationTest();

        //t.Delete(t.Root);

    }
    t.InOrder(t.Root);
    
    /*
    while (true)
    {
        try
        {
            switch (Console.ReadLine())
            {
                case "i":
                    t.Insert(new Node(int.Parse(Console.ReadLine())));
                    break;
                case "d":
                    t.Delete(t.Search(int.Parse(Console.ReadLine())));
                    break;
            }
        }
        catch
        {

        }
        t.InOrder(t.Root);
    }
    Console.WriteLine();
    */
}

static List<T> Shuffle<T>(List<T> list)
{
    var r = new Random();
    for (var i = 0; i < list.Count; i++)
    {
        var j = r.Next(list.Count);
        var temp = list[i];
        list[i] = list[j];
        list[j] = temp;
    }
    return list;
}


public enum RbColor
{
    Red,
    Black
}

internal class Node
{
    public int Key { get; set; }
    public Node? Left { get; set; }
    public Node? Right { get; set; }
    public Node? Parent { get; set; }
    public RbColor Color { get; set; }

    public Node(int key)
    {
        Key = key;
        Color = RbColor.Red;
        Left = null;
        Right = null;
        Parent = null;
    }
}

internal class RedBlackTree
{
    public Node? Root { get; set; }

    public int Size { get; set; }

    public void Insert(Node z)
    {
        Size++;
        Node y = null;
        Node x = this.Root;

        while (x != null)
        {
            y = x;
            if (z.Key < x.Key)
            {
                x = x.Left;
            }
            else
            {
                x = x.Right;
            }
        }

        z.Parent = y;

        if (y == null)
        {
            this.Root = z;
        }
        else if (z.Key < y.Key)
        {
            y.Left = z;
        }
        else
        {
            y.Right = z;
        }

        z.Left = null;
        z.Right = null;
        z.Color = RbColor.Red;

        InsertFixup(z);
    }

    private void InsertFixup(Node z)
    {
        while (z.Parent is { Color: RbColor.Red })
        {
            if (z.Parent.Parent != null && z.Parent == z.Parent.Parent.Left)
            {
                var y = z.Parent.Parent.Right;


                if (y is { Color: RbColor.Red })
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

                if (y is { Color: RbColor.Red })
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

    private void Transplant(Node u, Node? v)
    {
        if (u.Parent == null)
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
        if (v != null)
            v.Parent = u.Parent;
    }

    public void Delete(Node? z)
    {
        if (z == null)
        {
            return;
        }
        Size--;

        var y = z;
        var yOriginalColor = y.Color;
        Node? x;

        if (z.Left == null)
        {
            x = z.Right;
            Transplant(z, z.Right);
        }
        else if (z.Right == null)
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
                if (x != null)             //Check navíc
                    x.Parent = y;
            }
            else
            {
                Transplant(y, y.Right);
                if (z.Right != null)
                {
                    y.Right = z.Right;
                    y.Right.Parent = y;
                }
            }

            Transplant(z, y);
            y.Left = z.Left;
            y.Left.Parent = y;
            y.Color = z.Color;
        }

        if (yOriginalColor == RbColor.Black)
            DeleteFixup(x);
    }

    private void DeleteFixup(Node? x)
    {
        if (x == null) return;

        while (x != Root && x.Color == RbColor.Black)
        {
            if (x.Parent != null && x == x.Parent.Left) //Check parent != null
            {
                var w = x.Parent.Right;

                if (w is { Color: RbColor.Red }) //Check w != null
                {
                    w.Color = RbColor.Black;
                    x.Parent.Color = RbColor.Red;
                    LeftRotate(x.Parent);
                    w = x.Parent.Right;
                }
                if (w != null && (w.Left == null || w.Left.Color == RbColor.Black) && (w.Right == null || w.Right.Color == RbColor.Black)) //3 checks
                {
                    w.Color = RbColor.Red;
                    x = x.Parent;
                }
                else
                {
                    if (w != null && (w.Right == null || w.Right.Color == RbColor.Black)) //Check w.right == null w!=null
                    {
                        if (w.Left != null) //Check
                            w.Left.Color = RbColor.Black;
                        w.Color = RbColor.Red;
                        RightRotate(w);
                        w = x.Parent.Right;
                    }
                    if (w != null)
                        w.Color = x.Parent.Color;
                    x.Parent.Color = RbColor.Black;
                    if (w != null && w.Right != null)
                        w.Right.Color = RbColor.Black;
                    LeftRotate(x.Parent);
                    x = Root;
                }
            }
            else if (x.Parent != null)
            {
                var w = x.Parent.Left;

                if (w is { Color: RbColor.Red }) //Check w != null
                {
                    w.Color = RbColor.Black;
                    x.Parent.Color = RbColor.Red;
                    RightRotate(x.Parent);
                    w = x.Parent.Left; //TADY BYLO RIGHT melo byt?
                }
                if (w != null && (w.Right == null || w.Right.Color == RbColor.Black) && (w.Left == null || w.Left.Color == RbColor.Black)) //3 checks
                {
                    w.Color = RbColor.Red;
                    x = x.Parent;
                }
                else
                {
                    if (w != null && (w.Left == null || w.Left.Color == RbColor.Black)) //Check w.right == null w!=null
                    {
                        if (w.Right != null) //Check
                            w.Right.Color = RbColor.Black;
                        w.Color = RbColor.Red;
                        LeftRotate(w);
                        w = x.Parent.Left;
                    }
                    if (w != null)
                        w.Color = x.Parent.Color;
                    x.Parent.Color = RbColor.Black;
                    if (w != null && w.Left != null)
                        w.Left.Color = RbColor.Black;
                    RightRotate(x.Parent);
                    x = Root;
                }
            }
        }
        if (x != null)
            x.Color = RbColor.Black;
    }

    private static Node Minimum(Node node)
    {
        while (true)
        {
            if (node.Left == null) return node;
            node = node.Left;
        }
    }

    private void LeftRotate(Node? x)
    {
        if (x == null || x.Right == null) return; //zvláštní check pro x.right proč se LeftRotate volá v ten moment?

        var y = x.Right;
        x.Right = y.Left;
        if (y.Left != null)
        {
            y.Left.Parent = x;
        }
        y.Parent = x.Parent;
        if (x.Parent == null)
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

    private void RightRotate(Node? x)
    {
        if (x == null || x.Left == null) return; //zvláštní check pro x.left proč se RightRotate volá v ten moment?

        var y = x.Left;
        x.Left = y.Right;
        if (y.Right != null)
        {
            y.Right.Parent = x;
        }
        y.Parent = x.Parent;
        if (x.Parent == null)
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

    public void InOrder(Node node)
    {
        if (node == null) return;

        InOrder(node.Left);
        if (node.Color == RbColor.Red)
        {
            Console.ForegroundColor = ConsoleColor.Red;
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.White;
        }
        Console.Write(node.Key);
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write(", ");

        InOrder(node.Right);
    }

    private int Depth(Node? node)
    {
        return node == null ? 1 : Math.Max(Depth(node.Left), Depth(node.Right));
    }

    public void ValidationTest()
    {
        //TODO test 5. condition black nodes = all paths to leafs
        
        if (Root == null) return;

        if (Root != null && Root.Color == RbColor.Red)
        {
            throw new Exception("Root is red");
        }

        if (Depth(Root) - 1 > 2 * Math.Ceiling(Math.Log2((Size + 1))))
        {
            throw new Exception("Tree is too high");
        }

        ValidationTest(Root);
    }

    private void ValidationTest(Node? node)
    {
        if (node is null)
            return;

        if (node.Color == RbColor.Red && (node.Left != null && node.Left.Color == RbColor.Red || node.Right != null && node.Right.Color == RbColor.Red))
        {
            throw new Exception("Red node has red child");
        }

        ValidationTest(node.Left);
        ValidationTest(node.Right);
    }

    public Node Search(int key)
    {
        var node = Root;
        while (node != null)
        {
            if (node.Key == key)
                return node;
            if (node.Key < key)
                node = node.Right;
            else
                node = node.Left;
        }
        return null;
    }
}