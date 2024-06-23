namespace API_LesChevaliersEmeraude.Models.Custom
{
    public class Node<TData>
    {
        public Node<TData>? Parent;
        public TData Data { get; private set; }
        public List<Node<TData>> Children { get; } = new List<Node<TData>>();

        public Node(TData data, Node<TData>? parent = null, IEnumerable<Node<TData>>? children = null)
        {
            Data = data;

            Parent = parent;

            Children = children is null ? new List<Node<TData>>() : children.ToList();
        }

        public Node<TData> Add(TData data)
        {
            Node<TData> newChild = new Node<TData>(data, this);
            newChild.Parent = this;

            newChild.Parent = this;

            Children.Add(newChild);

            return newChild;
        }

        public Node<TData>? Find(Func<TData, bool> keySelector)
        {
            if (keySelector.Invoke(Data))
                return this;

            foreach (Node<TData> child in Children)
            {
                Node<TData>? toFind = child.Find(keySelector);

                if (toFind != null)
                    return toFind;
            }

            return null;
        }

        public Node<TData>? Cut(Func<TData, bool> keySelector)
        {
            Node<TData>? toCut = Find(keySelector);

            if (toCut == null)
                return null;

            if (toCut.Parent == null)
                return null;

            toCut.Parent.Children.Remove(toCut);

            return toCut;
        }
    }
}
