namespace API_LesChevaliersEmeraude.Models.Custom
{
    public class KnightNode
    {
        private readonly Chevalier knight;
        public SimpleKnight Knight
        {
            get => new SimpleKnight
            {
                Id = knight.IdChevalier,
                Name = knight.IdChevalierNavigation.Nom,
                Gender = knight.IdChevalierNavigation.Sexe.ToString(),
                BirthPlace = knight.IdChevalierNavigation.IdLieuOrigineNavigation?.Nom,
                HomePlace = knight.IdChevalierNavigation.IdLieuResidenceNavigation?.Nom,
                Generation = knight.Generation
            };
        }

        public KnightNode? Master { get => knight.IdMaitres.Select(master => new KnightNode(master)).FirstOrDefault();  }
        public KnightNode[] Squires { get => knight.IdEcuyers.Select(squire => new KnightNode(squire)).ToArray(); }

        public KnightNode(Chevalier knight) => this.knight = knight;

        public KnightNode? Find(int id)
        {
            if (Knight.Id == id)
                return this;

            foreach (KnightNode node in Squires)
            {
                KnightNode? toFind = node.Find(id);

                if (toFind != null)
                    return toFind;
            }

            return null;
        }

        public SimpleKnight[]? Cut(int id)
        {
            KnightNode? toCut = Find(id);

            if (toCut == null) 
                return null;

            KnightNode? master = toCut.Master;

            if (master == null)
                return Squires.Select(node => node.Knight).ToArray();

            return master.Squires.Select(node => node.Knight).ToArray();
        }

        public override string ToString() => $"{Knight.Name}";
    }
}
