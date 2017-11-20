using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Rainbow.ServiceDiscovery.Test
{
    public class ServiceDiscoveryReloadTokenTest
    {
        [Fact]
        public void ReloadTokenNotChangedEqual()
        {
            ServiceDiscoveryReloadToken token = new ServiceDiscoveryReloadToken();
            Assert.Equal(token.HasChanged, false);
            Assert.Equal(token.ActiveChangeCallbacks, true);
        }


        [Fact]
        public void ReloadTokenChangedEqual()
        {
            ServiceDiscoveryReloadToken token = new ServiceDiscoveryReloadToken();
            token.OnReload();
            Assert.Equal(token.HasChanged, true);
        }

    }
}
