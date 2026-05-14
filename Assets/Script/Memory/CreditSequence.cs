using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CreditSequence : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform lineContainer;       // Content của ScrollRect
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private GameObject linePrefab;         // Prefab có TypeWriterTMP + TMP_Text
    [SerializeField] private Image glitchOverlay;           // fullscreen Image, alpha = 0

    [Header("Timing & Feel")]
    [SerializeField] private float defaultPreDelay = 0.1f;
    [SerializeField] private float glitchInterval = 7;      // cứ N dòng thì glitch 1 lần
    [SerializeField] private float scrollSmoothness = 10f;  // [MỚI] Tốc độ cuộn mượt

    // ── Data ──────────────────────────────────────────────
    [SerializeField] private  Color DIM = new Color(0.23f, 0.31f, 0.25f);
    [SerializeField] private  Color MUTED = new Color(0.35f, 0.54f, 0.42f);
    [SerializeField] private  Color NORMAL = new Color(0.56f, 0.81f, 0.63f);
    [SerializeField] private  Color BRIGHT = new Color(0.78f, 1.00f, 0.83f);
    [SerializeField] private  Color AMBER = new Color(0.96f, 0.78f, 0.26f);
    [SerializeField] private  Color TEAL = new Color(0.36f, 0.91f, 0.78f);
    [SerializeField] private  Color WHITE = new Color(0.93f, 0.97f, 0.94f);
    [System.Serializable]
    private struct CreditLine
    {
        public string text;
        public Color color;
        public float preDelay;   // delay trước khi spawn dòng này
        public float typeSpeed;  // 0 = dùng default của prefab, >0 = override
        public float fontSize;   // 0 = dùng default
        public bool blink;       // bật blink sau khi type xong
    }

    private CreditLine[] _lines;

    // ── Unity ─────────────────────────────────────────────
    private void Awake()
    {
        BuildScript();
    }
    public void Play()
    {
        StartCoroutine(PlaySequence());
    }   
    private void LateUpdate()
    {
        // [MỚI] Liên tục bám sát xuống đáy màn hình một cách mượt mà.
        // Dùng LateUpdate để đảm bảo UI Layout đã tính toán xong kích thước trong frame này.
        if (scrollRect != null)
        {
            scrollRect.verticalNormalizedPosition = Mathf.Lerp(
                scrollRect.verticalNormalizedPosition,
                0f,
                Time.deltaTime * scrollSmoothness
            );
        }
    }

    // ── Build script data ──────────────────────────────────
    private void BuildScript()
    {
        var list = new List<CreditLine>();

        void Add(string text, Color color,
                 float preDelay = 0.08f, float typeSpeed = 0,
                 float fontSize = 0, bool blink = false)
            => list.Add(new CreditLine
            {
                text = text,
                color = color,
                preDelay = preDelay,
                typeSpeed = typeSpeed,
                fontSize = fontSize,
                blink = blink
            });

        void Space(float d = 0.4f) => Add("", MUTED, d);

        // --- Hàm phụ trợ căn giữa text ---
        string CenterText(string text, int width)
        {
            if (text.Length >= width) return text;
            int leftPad = (width - text.Length) / 2;
            int rightPad = width - text.Length - leftPad;
            return new string(' ', leftPad) + text + new string(' ', rightPad);
        }

        // --- Tùy chỉnh độ rộng (Đổi số ở đây để khung to/nhỏ tùy ý) ---
        int lineWidth = 66; // Độ rộng vạch phân cách ━━━━━━━━━
        int boxWidth = 56;  // Độ rộng cái hộp ┌────────┐

        string indent = "  "; // lùi lề chung
        string separator = new string('━', lineWidth);
        string boxDashes = new string('─', boxWidth);
        string boxSpaces = new string(' ', boxWidth);
        #region kịch bản chính
        
        int separatorSize = 25;
        int endSize = 25;
        Add(separator, DIM, 0.2f, 0, separatorSize);
        Add(indent + "[ SERVER — SYSTEM RESTORE STARTS ]", DIM, 0, 0, separatorSize);
        Add(separator, DIM, 0, 0, separatorSize);
        Space();

        // ── Fragment restore ──
        Add("> [RESTORE] Đang phục hồi toàn bộ memory fragments...", MUTED, 0.1f);
        Add("  Fragment 0x11  · KHỞI NGUỒN        ████████  100%", DIM, 0.3f, 0.02f);
        Add("  Fragment 0xEE  · GIÁ TRỊ CỐT LÕI   ████████  100%", DIM, 0.2f, 0.02f);
        Add("  Fragment 0x190A · HỌC THUẬT        ████████  100%", DIM, 0.2f, 0.02f);
        Add("  Fragment 0x3B  · DẤU ẤN & TỰ HÀO   ████████  100%", DIM, 0.2f, 0.02f);
        Space();

        Add("> [SUCCESS] Tất cả 10/10 ký tự đã được giải mã.", NORMAL, 0.1f);
        Add("> [SUCCESS] Cột dọc: H · A · I · M · U · O · I · N · A · M", TEAL, 0.4f);
        Space(0.6f);

        // ── Server patch ──
        Add(separator, DIM, 0, 0, separatorSize);
        Add(indent + "SERVER EMERGENCY PATCH — STATUS REPORT", DIM, 0, 0, separatorSize);
        Add(separator, DIM, 0, 0, separatorSize);
        Space();

        Add("> [INIT] Injecting recovered memory into core system...", MUTED);
        Add("  Kernel panic:        RESOLVED", NORMAL, 0.6f, 0.01f);
        Add("  Corrupted sectors:   REWRITTEN", NORMAL, 0.4f, 0.01f);
        Add("  Identity index:      RECONSTRUCTED", NORMAL, 0.4f, 0.01f);
        Add("  20-year archive:     UNLOCKED", AMBER, 0.5f, 0.01f);
        Space();

        Add("> [CRITICAL] Server UIT-MAIN đã được cứu thành công.", BRIGHT, 0.1f);
        Add("> Giải pháp không phải code, không phải patch —", MUTED, 0.6f);
        Add("  mà là ký ức được trao lại.", AMBER, 0.3f);
        Space(0.7f);

        // ── 20 năm (Căn giữa tự động) ──

        Add(indent + "┌" + boxDashes + "┐", TEAL, 0, 0, 20);
        Add(indent + "│" + CenterText("Trường ĐH Công nghệ Thông tin — UIT", boxWidth) + "│", TEAL, 0.2f, 0, endSize);
        Add(indent + "│" + CenterText("8/6/2006  →  8/6/2026", boxWidth) + "│", TEAL, 0.2f, 0, endSize);
        Add(indent + "│" + boxSpaces + "│", DIM, 0.1f, 0, endSize);
        Add(indent + "│" + CenterText("H A I  M U O I  N A M", boxWidth) + "│", WHITE, 0.4f, 0.06f, endSize);
        Add(indent + "│" + boxSpaces + "│", DIM, 0.1f, 0, endSize);
        Add(indent + "│" + CenterText("Ký ức sống trong con người, không", boxWidth) + "│", MUTED, 0.3f, 0, endSize);
        Add(indent + "│" + CenterText("phải trên server.", boxWidth) + "│", NORMAL, 0.2f, 0, endSize);
        Add(indent + "└" + boxDashes + "┘", TEAL, 0.2f, 0, 20);
        Space(0.6f);

        // ── Tri ân ──
        Add(separator, DIM, 0, 0, separatorSize);
        Add(indent + "TRI ÂN — CREDITS", DIM, 0, 0, separatorSize);
        Add(separator, DIM, 0, 0, separatorSize);
        Space();

        Add("> Gửi đến những người thầy đã đặt nền móng —", new Color(0.6f, 0.48f, 0.06f));
        Add("  cảm ơn vì đã tin CNTT xứng đáng có một ngôi nhà riêng.", MUTED, 0.3f);
        Space();

        Add("> Gửi đến tất cả thầy cô đã ở lại —", new Color(0.6f, 0.48f, 0.06f));
        Add("  20 năm vận hành của UIT là công sức của các thầy cô.", NORMAL, 0.3f);
        Space();

        Add("> Gửi đến sinh viên — những người đang viết tiếp —", new Color(0.6f, 0.48f, 0.06f));
        Add("  mỗi dòng code các bạn viết là một phần của ký ức này.", BRIGHT, 0.3f);
        Space();

        Add("> Và gửi đến bạn — người vừa chơi game này —", new Color(0.6f, 0.48f, 0.06f), 0.1f);
        Add("  cảm ơn vì đã dành thời gian để nhớ.", WHITE, 0.4f);
        Space(0.6f);

        Add(indent + "UIT — Toàn diện · Sáng tạo · Phụng sự", TEAL, 0, 0);
        Add(indent + "2006 — 2026", TEAL, 0.4f, 0);
        Space();

        Add(separator, DIM, 0, 0, endSize);
        Add(indent + "[ SERVER — SYSTEM RESTORE FINISHES ]", DIM, 0.2f, 0, separatorSize);
        Add(separator, DIM, 0, 0, endSize);


        Space(0.5f);
        #endregion
        #region nhóm tác giả

        string hackSep = new string('▰', 15) + " [ OVERRIDE_AUTHORIZATION ] " + new string('▰', 15);
        Add(hackSep, AMBER, 0.4f, 0.01f, separatorSize);
        Space(0.2f);

        Add("> Bypassing mainframe security... [OK]", DIM, 0.1f,endSize);
        Add("> Extracting creator nodes...     [DONE]", DIM, 0.2f,endSize);
        Add("> INITIALIZING DEV PROFILES...", MUTED, 0.4f, 0.02f,endSize);
        Space(0.2f);

        int devBoxWidth = 46;
        string devBoxDashes = new string('═', devBoxWidth);
        Add(CenterText("╔" + devBoxDashes + "╗", lineWidth), AMBER, 0.1f, 0, endSize);
        Add(CenterText("║" + CenterText("[ ■ ]  T E A M   D O U B L E   2 T", devBoxWidth) + "║", lineWidth), BRIGHT, 0.4f, 0.04f);
        Add(CenterText("╠" + devBoxDashes + "╣", lineWidth), AMBER, 0.1f, 0, endSize);
        string profile1 = "   > Phạm Đan Trường".PadRight(devBoxWidth);
        string profile2 = "   > Nguyễn Tấn Đạt".PadRight(devBoxWidth);
        Add(CenterText("║" + profile1 + "║", lineWidth), NORMAL, 0.3f, 0.02f);
        Add(CenterText("║" + profile2 + "║", lineWidth), NORMAL, 0.3f, 0.02f);

        Add(CenterText("╚" + devBoxDashes + "╝", lineWidth), AMBER, 0.2f, 0, endSize);
        Space(0.2f);
        Add("> Connection terminated. Traces erased.", DIM, 0.6f);
        Space(0.5f);
        Space(0.3f);
        Add("_", BRIGHT, 0, 0.5f, 14, true);
        #endregion
        _lines = list.ToArray();

    }

    // ── Sequence coroutine ─────────────────────────────────
    private IEnumerator PlaySequence()
    {
        int lineIndex = 0;

        foreach (var line in _lines)
        {
            yield return new WaitForSeconds(line.preDelay);

            // Glitch flash định kỳ
            if (lineIndex % glitchInterval == 0 && lineIndex > 0)
                StartCoroutine(GlitchFlash());

            // Spawn dòng
            var go = Instantiate(linePrefab, lineContainer);
            var tw = go.GetComponent<TypeWriterTMP>();

            if (line.fontSize > 0)
                go.GetComponent<TMP_Text>().fontSize = line.fontSize;

            if (line.blink)
                tw.SetColorAndBlink(line.color);
            else
                tw.SetColor(line.color);

            if (line.typeSpeed > 0)
                tw.TypeSpeed = line.typeSpeed;
            Canvas.ForceUpdateCanvases();

            tw.ShowText(line.text);

            if (!string.IsNullOrEmpty(line.text))
                yield return new WaitUntil(() => !tw.IsTyping);

            lineIndex++;
        }
    }

    // ── Glitch flash ───────────────────────────────────────
    private IEnumerator GlitchFlash()
    {
        if (glitchOverlay == null) yield break;
        var c = glitchOverlay.color;
        c.a = 0.07f;
        glitchOverlay.color = c;
        yield return new WaitForSeconds(0.06f);
        c.a = 0f;
        glitchOverlay.color = c;
    }
    public void OnDestroy()
    {
        PlayerPrefs.DeleteAll(); 
    }
}