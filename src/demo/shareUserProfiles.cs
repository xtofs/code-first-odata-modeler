using System.ComponentModel.DataAnnotations;

namespace sharedUserProfiles;

public record outboundSharedUserProfile(
    [property: Key] string userId,
    IReadOnlyCollection<outboundSharedUserProfileTenant> tenants
    )
{ }

public record outboundSharedUserProfileTenant(
    [property: Key] string tenantId
    )
{ }


public record Service(
     IReadOnlyCollection<outboundSharedUserProfile> OutboundSharedUserProfiles
)
{ }