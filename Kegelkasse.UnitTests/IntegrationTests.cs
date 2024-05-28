using Strafenkatalog.Models;
using Strafenkatalog.ViewModel;

namespace Kegelkasse.UnitTests
{
    public class IntegrationTests
    {
        
        private readonly StrafenkatalogContext context;

        public IntegrationTests()
        {
            this.context = new StrafenkatalogContext();
        }

        [Test]
        public void CheckErrorInput_Success()
        {
            var gamePlayer = this.context.GamePlayers.First();
            var playerPenalties = this.context.PlayerPenalties.Where(pp => pp.GamePlayer == gamePlayer.Id).ToList();

            foreach (var item in playerPenalties)
            {
                item.PenaltyNavigation = this.context.Penalties.Where(p => p.Id == item.Penalty).First();
            }

            var vm = new EditPenaltyViewModel(gamePlayer, playerPenalties);
            Assert.That(vm, Is.Not.Null);
            const int newErrorValue = 10;
            vm.Errors = newErrorValue;

            var errorPenalty = vm.PlayerPenaltyViewModels.Where(ppvm => !ppvm.IsNotErrorPenalty).FirstOrDefault();
            Assert.That(errorPenalty, Is.Not.Null);
            Assert.That(errorPenalty.Value, Is.EqualTo(newErrorValue));
            Assert.Pass();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            context.Dispose();
        }
    }
}