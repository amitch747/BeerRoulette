using RimWorld;
using Verse;

namespace BeerRoulette
{
    public class CompProperties_BeerRoulette : CompProperties
    {
        public float explosionChance = 1f / 6f;
        public float explosionRadius = 1.9f;

        public CompProperties_BeerRoulette()
        {
            compClass = typeof(CompBeerRoulette);
        }
    }

    public class CompBeerRoulette : ThingComp
    {
        // Get props
        public CompProperties_BeerRoulette Props => (CompProperties_BeerRoulette)props;

        public override void PostIngested(Pawn ingester)
        {
            // normal beer
            base.PostIngested(ingester); 
            // special beer
            if (Rand.Value < Props.explosionChance) 
            {
                GenExplosion.DoExplosion(
                    center: ingester.Position,
                    map: ingester.Map,
                    radius: Props.explosionRadius,
                    damType: DamageDefOf.Bomb,
                    instigator: ingester
                );
            }
        }
    }
}
