Random r = new();

for (int j = 0; j < 100; j++)
{
    RedBlackTree t = new();
    for (int i = 0; i < 1000; i++)
    {
        t.Insert(new(r.Next(100)));
        t.ValidationTest();
    }
    
    for (int i = 0; i < 1000; i++)
    {
        t.Delete(t.Root);
        t.ValidationTest();
    }

    t.InOrder(t.Root);

    Console.WriteLine();
}

public enum RBColor{
    Red,
    Black
}

class Node
{
    public int Key { get; set; }
    public Node Left { get; set; }
    public Node Right { get; set; }
    public Node Parent { get; set; }
    public RBColor Color { get; set; }

    public Node(int key)
    {
        Key = key;
        Color = RBColor.Red;
        Left = null;
        Right = null;
        Parent = null;
    }
}

class RedBlackTree
{
    public Node Root { get; set; }
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

        if(y == null)
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
        z.Color = RBColor.Red;

        InsertFixup(z);
    }

    private void InsertFixup(Node z)
    {
        while(z.Parent != null && z.Parent.Color == RBColor.Red)
        {
            if(z.Parent.Parent != null && z.Parent == z.Parent.Parent.Left)
            {
                Node y = z.Parent.Parent.Right;
                

                if(y != null && y.Color == RBColor.Red)
                {
                    z.Parent.Color = RBColor.Black;
                    y.Color = RBColor.Black;
                    z.Parent.Parent.Color = RBColor.Red;
                    z = z.Parent.Parent;
                }
                else
                {
                    if (z == z.Parent.Right)
                    {
                        z = z.Parent;
                        LeftRotate(z);
                    }

                    z.Parent.Color = RBColor.Black;
                    z.Parent.Parent.Color = RBColor.Red;
                    RightRotate(z.Parent.Parent);
                }
            }
            else if(z.Parent.Parent != null)
            {
                Node y = z.Parent.Parent.Left;

                if (y != null && y.Color == RBColor.Red)
                {
                    z.Parent.Color = RBColor.Black;
                    y.Color = RBColor.Black;
                    z.Parent.Parent.Color = RBColor.Red;
                    z = z.Parent.Parent;
                }
                else
                {
                    if (z == z.Parent.Left)
                    {
                        z = z.Parent;
                        RightRotate(z);
                    }

                    z.Parent.Color = RBColor.Black;
                    z.Parent.Parent.Color = RBColor.Red;
                    LeftRotate(z.Parent.Parent);
                }
            }
        }

        Root.Color = RBColor.Black;
    }

    private void Transplant(Node u, Node v)
    {
        if(u.Parent == null) 
        { 
            Root = v; 
        }
        else if(u == u.Parent.Left)
        {
            u.Parent.Left = v;
        }
        else
        {
            u.Parent.Right = v;
        }
        if(v != null)
            v.Parent = u.Parent;
    }

    public void Delete(Node z)
    {
        Size--;
        var y = z;
        var yOriginalColor = y.Color;
        Node x = null;

        if(z.Left == null)
        {
            x = z.Right;
            Transplant(z, z.Right);
        }
        else if(z.Right == null)
        {
            x = z.Left;
            Transplant(z, z.Left);
        }
        else
        {
            y = Minimum(z.Right);
            yOriginalColor = y.Color;
            x = y.Right;
            if(x != null && y.Parent == z)
            {
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

        if (yOriginalColor == RBColor.Black)
            DeleteFixup(x);
    }

    private void DeleteFixup(Node x)
    {
        while( x != null && x != Root && x.Color == RBColor.Black)
        {
            if (x == x.Parent.Left)
            {
                var w = x.Parent.Right;

                if (w.Color == RBColor.Red)
                {
                    w.Color = RBColor.Black;
                    x.Parent.Color = RBColor.Red;
                    LeftRotate(x.Parent);
                    w = x.Parent.Right;
                }
                if (w.Left.Color == RBColor.Black && w.Right.Color == RBColor.Black)
                {
                    w.Color = RBColor.Red;
                    x = x.Parent;
                }
                else
                {
                    if (w.Right.Color == RBColor.Black)
                    {
                        w.Left.Color = RBColor.Black;
                        w.Color = RBColor.Red;
                        RightRotate(w);
                        w = x.Parent.Right;
                    }
                    w.Color = x.Parent.Color;
                    x.Parent.Color = RBColor.Black;
                    w.Right.Color = RBColor.Black;
                    LeftRotate(x.Parent);
                    x = Root;
                }
            }
            else
            {
                var w = x.Parent.Left;

                if (w.Color == RBColor.Red)
                {
                    w.Color = RBColor.Black;
                    x.Parent.Color = RBColor.Red;
                    RightRotate(x.Parent);
                    w = x.Parent.Left;
                }
                if (w.Right.Color == RBColor.Black && w.Left.Color == RBColor.Black)
                {
                    w.Color = RBColor.Red;
                    x = x.Parent;
                }
                else
                {
                    if (w.Left.Color == RBColor.Black)
                    {
                        w.Right.Color = RBColor.Black;
                        w.Color = RBColor.Red;
                        LeftRotate(w);
                        w = x.Parent.Left;
                    }
                    w.Color = x.Parent.Color;
                    x.Parent.Color = RBColor.Black;
                    w.Left.Color = RBColor.Black;
                    RightRotate(x.Parent);
                    x = Root;
                }
            }
        }
        if(x != null)
            x.Color = RBColor.Black;
    }

    private Node Minimum(Node node)
    {
        if (node.Left != null)
            return Minimum(node.Left);
        else
            return node;
    }

    private void LeftRotate(Node x)
    {
        var y = x.Right;
        x.Right = y.Left;
        if(y.Left != null)
        {
            y.Left.Parent = x;
        }
        y.Parent = x.Parent;
        if(x.Parent == null)
        {
            Root = y;
        }
        else if(x == x.Parent.Left)
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
        if(node.Color == RBColor.Red)
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

    private int Depth(Node node)
    {
        if (node == null)
            return 1;
        return Math.Max(Depth(node.Left), Depth(node.Right));
    }

    public void ValidationTest()
    {
        //Doesn't include path to leafs test
        if (Root != null && Root.Color == RBColor.Red)
        {
            throw new Exception("Root is red");
        }

        if (Depth(Root) - 1 > 2 * Math.Ceiling(Math.Log2((Size + 1))))
        {
            throw new Exception("Tree is too high");
        }

        ValidationTest(Root);
    }

    private void ValidationTest(Node node)
    {
        if (node is null)
            return;

        if (node.Color == RBColor.Red && (node.Left != null && node.Left.Color == RBColor.Red || node.Right != null && node.Right.Color == RBColor.Red))
        {
            throw new Exception("Red node has red child");
        }

        ValidationTest(node.Left);
        ValidationTest(node.Right);
    }
}