using RimWorld;
using Verse;

namespace BeerRoulette
{
    public class CompProperties_BeerRoulette : CompProperties
    {
        public float explosionChance = 1f / 10f;
        public float explosionRadius = 0.9f;

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
            //Log.Message($"[BeerRoulette] {ingester.LabelShort} took a chance");
            // special beer
            if (Rand.Value < Props.explosionChance) 
            {
                //Log.Message($"[BeerRoulette] {ingester.LabelShort} was taken by chance");

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

    public class Verb_MeleeBeerRoulette : Verb_MeleeAttackDamage
    {
        public Verb_MeleeBeerRoulette()
        {
            Log.Message("[BeerRoulette] Verb_MeleeBeerRoulette constructed");
        }

        protected override DamageWorker.DamageResult ApplyMeleeDamageToTarget(LocalTargetInfo target)
        {
            //Log.Message($"[BeerRoulette] ApplyMeleeDamageToTarget called. Tool={tool?.label ?? "null"}, Caster={CasterPawn?.LabelShort ?? "null"}");

            DamageWorker.DamageResult damageResult = base.ApplyMeleeDamageToTarget(target);

            CompBeerRoulette comp = EquipmentSource?.GetComp<CompBeerRoulette>();
            //Log.Message($"[BeerRoulette] EquipmentSource={EquipmentSource?.Label ?? "null"}, comp={(comp != null ? "found" : "null")}");

            if (comp != null && Rand.Value < comp.Props.explosionChance)
            {
                //Log.Message($"[BeerRoulette] EXPLODING on {target.Label}");
                GenExplosion.DoExplosion(
                    center: target.Cell,
                    map: CasterPawn.Map,
                    radius: comp.Props.explosionRadius,
                    damType: DamageDefOf.Bomb,
                    instigator: CasterPawn
                );
            }

            return damageResult;
        }
}

}
