using System.Reflection;
using System.Runtime.Serialization;
using HardlightProject;
using NUnit.Framework;

namespace ProjectLucid.Tests.EditMode
{
    public sealed class CharacterAbilityRollingTests
    {
        // Original 060011c5 (1.10.1 RVA 0x74B370) stores a zero word at +0xE8 after ActorAbility.DoOnLeave,
        // clearing IsRollingTriggered (+0xE8) and IsRollingExiting (+0xE9) together.
        [Test]
        public void LeaveClearsTriggeredAndExitingFlags()
        {
            // Constructors are skipped: the CharacterAbility field initializer resolves the App system reference,
            // and DoOnLeave reads no other state.
            var ability = (CharacterAbility_Rolling)FormatterServices.GetUninitializedObject(typeof(CharacterAbility_Rolling));
            PropertyInfo triggered = typeof(CharacterAbility_Rolling).GetProperty(nameof(CharacterAbility_Rolling.IsRollingTriggered));
            PropertyInfo exiting = typeof(CharacterAbility_Rolling).GetProperty(nameof(CharacterAbility_Rolling.IsRollingExiting));
            MethodInfo leave = typeof(CharacterAbility_Rolling).GetMethod("DoOnLeave", BindingFlags.Instance | BindingFlags.NonPublic);

            // State left by UpdateTrigger(true) then UpdateTrigger(false) while RollingActive is false.
            triggered.SetValue(ability, false);
            exiting.SetValue(ability, true);
            leave.Invoke(ability, null);
            Assert.That(ability.IsRollingTriggered, Is.False);
            Assert.That(ability.IsRollingExiting, Is.False);

            triggered.SetValue(ability, true);
            exiting.SetValue(ability, true);
            leave.Invoke(ability, null);
            Assert.That(ability.IsRollingTriggered, Is.False);
            Assert.That(ability.IsRollingExiting, Is.False);
        }
    }
}
