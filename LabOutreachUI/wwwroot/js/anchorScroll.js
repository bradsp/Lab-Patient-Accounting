// Same-page fragment ("#section") links — e.g. the Table of Contents in the Help
// documentation — must scroll to the target heading rather than being hijacked by
// Blazor's navigation interception (which treats them as a route change and sends the
// user back to the app root).
//
// Blazor listens for anchor clicks in the bubbling phase on `document`. Registering
// this handler in the CAPTURE phase lets it run first; stopPropagation() then prevents
// Blazor's handler from firing at all for pure in-page fragment links.
(function () {
    function scrollToHash(hash) {
        if (!hash || hash === '#') return false;
        var id = decodeURIComponent(hash.substring(1));
        var el = document.getElementById(id);
        if (!el) {
            var named = document.getElementsByName(id);
            if (named && named.length) el = named[0];
        }
        if (el) {
            el.scrollIntoView({ behavior: 'smooth', block: 'start' });
            return true;
        }
        return false;
    }

    document.addEventListener('click', function (e) {
        var anchor = e.target && e.target.closest ? e.target.closest('a') : null;
        if (!anchor) return;

        // Only handle pure fragment links (href starts with "#"), which is exactly what
        // Markdig renders for in-document Table of Contents entries. Leave all real
        // navigation links for Blazor to handle normally.
        var href = anchor.getAttribute('href');
        if (!href || href.charAt(0) !== '#') return;

        if (scrollToHash(href)) {
            e.preventDefault();
            e.stopPropagation();
            // Reflect the anchor in the address bar without triggering navigation,
            // so the link is copyable/bookmarkable.
            if (window.history && window.history.replaceState) {
                window.history.replaceState(null, '', location.pathname + location.search + href);
            }
        }
    }, true); // capture phase — runs before Blazor's bubbling-phase handler
})();
