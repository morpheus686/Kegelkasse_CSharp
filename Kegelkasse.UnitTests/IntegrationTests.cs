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

        [Test]
        public void CalculateTotalResult_Success()
        {
            var gamePlayer = this.context.GamePlayers.First();
            var vm = new EditPenaltyViewModel(gamePlayer, Enumerable.Empty<PlayerPenalty>());

            var originalClearValue = vm.Clear;
            var originalFullValue = vm.Full;
            var originalTotalResultValue = vm.Result;

            var newFullValue = originalFullValue + 1;
            vm.Full = newFullValue;
            Assert.That(vm.Result, Is.EqualTo(originalTotalResultValue + 1));

            var newClearValue = originalClearValue - 1;
            vm.Clear = newClearValue;
            Assert.That(vm.Result, Is.EqualTo(originalTotalResultValue));
            Assert.Pass();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            context.Dispose();
        }
    }
}