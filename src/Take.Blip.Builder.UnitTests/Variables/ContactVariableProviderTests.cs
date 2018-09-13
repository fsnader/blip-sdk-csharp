﻿using Lime.Messaging.Resources;
using Lime.Protocol;
using NSubstitute;
using Shouldly;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Take.Blip.Builder.Variables;
using Take.Blip.Client.Extensions.Contacts;
using Xunit;

namespace Take.Blip.Builder.UnitTests.Variables
{
    public class ContactVariableProviderTests : CancellationTokenTestsBase
    {
        public ContactVariableProviderTests()
        {
            ContactExtension = Substitute.For<IContactExtension>();
            Context = Substitute.For<IContext>();
            InputContext = new Dictionary<string, object>();
            Context.InputContext.Returns(InputContext);
            Contact = new Contact()
            {
                Identity = "john@domain.com",
                Name = "John Doe",
                Address = "184 Alpha Avenue"
            };
            ContactExtension.GetAsync(Contact.Identity, CancellationToken).Returns(Contact);
            Context.User.Returns(Contact.Identity);
        }

        public IContactExtension ContactExtension { get; }

        public IContext Context { get;  }

        public IDictionary<string, object> InputContext { get; }

        public Contact Contact { get;  }

        public ContactVariableProvider GetTarget()
        {
            return new ContactVariableProvider(ContactExtension);
        }

        [Fact]
        public async Task GetNameWhenContactExistsShouldReturnName()
        {
            // Arrange
            var target = GetTarget();

            // Act
            var actual = await target.GetVariableAsync("name", Context, CancellationToken);

            // Asset
            actual.ShouldBe(Contact.Name);
        }

        [Fact]
        public async Task GetNameWhenContactDoesNotExistsShouldReturnNull()
        {
            // Arrange
            var target = GetTarget();

            Contact nullContact = null;        
            ContactExtension.GetAsync(Arg.Any<Identity>(), Arg.Any<CancellationToken>()).Returns(nullContact);

            // Act
            var actual = await target.GetVariableAsync("name", Context, CancellationToken);

            // Asset
            actual.ShouldBeNull();
        }


        [Fact]
        public async Task GetNameTwiceWhenContactExistsShouldUseCachedValue()
        {
            // Arrange
            var target = GetTarget();

            // Act
            var actualName = await target.GetVariableAsync("name", Context, CancellationToken);
            var actualADdress = await target.GetVariableAsync("address", Context, CancellationToken);

            // Asset
            actualName.ShouldBe(Contact.Name);
            actualADdress.ShouldBe(Contact.Address);
            ContactExtension.Received(1).GetAsync(Arg.Any<Identity>(), Arg.Any<CancellationToken>());
        }
    }
}