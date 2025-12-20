using AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.Sites;
using System.Text.Json.Serialization;

namespace AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.Accounts;

public class AccountResponse
{
    public int Id { get; set; }

    [JsonPropertyName("account_local_id")]
    public int AccountLocalId { get; set; }

    [JsonPropertyName("site_id")]
    public int SiteId { get; set; }

    [JsonPropertyName("role_id")]
    public int RoleId { get; set; }

    [JsonPropertyName("distributor_id")]
    public int DistributorId { get; set; }

    public string Status { get; set; } = default!;

    [JsonPropertyName("last_login_at")]
    public DateTime LastLoginAt { get; set; }

    [JsonPropertyName("user_id")]
    public int UserId { get; set; }

    [JsonPropertyName("session_timeout_min")]
    public int SessionTimeoutMin { get; set; }

    [JsonPropertyName("inviter_account_id")]
    public int InviterAccountId { get; set; }

    [JsonPropertyName("inviter_email")]
    public string InviterEmail { get; set; } = default!;

    [JsonPropertyName("inviter_role")]
    public string InviterRole { get; set; } = default!;

    [JsonPropertyName("InvitedAt")]
    public DateTime InvitedAt { get; set; }

    [JsonPropertyName("alert_status")]
    public int AlertStatus { get; set; }

    [JsonPropertyName("alert_email")]
    public int AlertEmail { get; set; }

    [JsonPropertyName("alert_sms")]
    public int AlertSms { get; set; }

    [JsonPropertyName("alert_app")]
    public int AlertApp { get; set; }

    [JsonPropertyName("alert_updated_at")]
    public DateTime AlertUpdatedAt { get; set; }

    [JsonPropertyName("popup_firmware_disabled_at")]
    public DateTime PopupFirmwareDisabledAt { get; set; }

    [JsonPropertyName("fcm_token")]
    public string FcmToken { get; set; } = default!;

    public int Favorite { get; set; }

    [JsonPropertyName("favorite_at")]
    public DateTime FavoriteAt { get; set; }

    public string Properties { get; set; } = default!;

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("updated_at")]
    public DateTime UpdatedAt { get; set; }

    public string Mobile { get; set; } = default!;

    [JsonPropertyName("role_type")]
    public string RoleType { get; set; } = default!;

    public SiteResponse Site { get; set; } = default!;
}
