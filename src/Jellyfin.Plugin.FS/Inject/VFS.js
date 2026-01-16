// Scope everything in a check to avoid re-declaring the plugin
if (typeof window.VFSPlugin == 'undefined') {

    // Define the plugin on the window object for universal access
    window.VFSPlugin = {
        initialized: false,
        currentPage: null,

        // Kicks off the process
        init: function () {
            console.log('VFS: Initializing plugin');
            this.waitForUI();
        },

        // Waits for the necessary page elements to be ready before acting
        waitForUI: function () {
            // Check if we are on the home page by looking at the URL hash
            const hash = window.location.hash;
            if (hash !== '' && hash !== '#/home' && hash !== '#/home.html' && !hash.includes('#/home?') && !hash.includes('#/home.html?')) {
                console.debug('VFS: Not on main page, skipping UI check. Hash:', hash);
                return;
            }

            // If the UI is ready, create tabs; otherwise, wait and check again
            if (typeof ApiClient !== 'undefined' && document.querySelector('.emby-tabs-slider')) {
                console.debug('VFS: UI elements available on main page, creating tabs');
                this.createCustomTabs();
            } else {
                console.debug('VFS: ',ApiClient,document.querySelector('.emby-tabs-slider'))
                console.debug('VFS: Waiting for UI elements on main page...');
                setTimeout(() => this.waitForUI(), 200);
            }
        },

        // Fetches config and creates the tab elements in the DOM
        createCustomTabs: function () {
            console.debug('VFS: Starting tab creation process');

            const tabsSlider = document.querySelector('.emby-tabs-slider');
            if (!tabsSlider) {
                console.debug('VFS: Tabs slider not found');
                return;
            }

            // Prevent creating duplicate tabs if they already exist
            if (tabsSlider.querySelector('[id="VFS_Index"]')) {
                console.debug('VFS: VFS already exist in DOM, skipping creation');
                return;
            }

            // // Fetch tab configuration from the server
            // ApiClient.fetch({
            //     url: ApiClient.getUrl('fs/api/storage/init'),
            //     type: 'GET',
            //     dataType: 'json',
            //     headers: {
            //         accept: 'application/json'
            //     }
            // }).then((configs) => {
                // console.debug('VFS: Retrieved config for', configs.length, 'tabs');

                // const tabsSlider = document.querySelector('.emby-tabs-slider');
                // if (!tabsSlider) {
                //     console.error('VFS: Tabs slider disappeared unexpectedly');
                //     return;
                // }

                // Loop through configs and create a tab for each one

                const customTabId = `VFS_Index`;

                // Final check to ensure this specific tab doesn't already exist
                if (document.querySelector(`#${customTabId}`)) {
                    console.debug(`VFS: Tab ${customTabId} already exists, skipping`);
                    return; // 'return' here acts like 'continue' in a forEach loop
                }

                // console.log("VFS: Creating custom tab:", config.Title);

                const title = document.createElement("div");
                title.classList.add("emby-button-foreground");
                title.innerText = "VFS";

                const button = document.createElement("button");
                button.type = "button";
                button.setAttribute("is", "empty-button");
                button.classList.add("emby-tab-button", "emby-button");
                button.setAttribute("id", customTabId);
                button.appendChild(title);

                tabsSlider.appendChild(button);
                console.log(`VFS: Added tab ${customTabId} to tabs slider`);


                console.log('VFS: All custom tabs created successfully');
            // }).catch((error) => {
            //     console.error('VFS: Error fetching tab configs:', error);
            // });
        }
    };

    // --- Event Listeners to Handle Navigation ---

    // Initial setup when the page is first loaded
    if (document.readyState === 'loading') {
        document.addEventListener("DOMContentLoaded", () => window.VFSPlugin.init());
    } else {
        window.VFSPlugin.init();
    }

    // A single handler for all navigation-style events
    const handleNavigation = () => {
        console.debug('VFS: Navigation detected, re-initializing after delay');
        // Delay helps ensure the DOM has settled after navigation
        setTimeout(() => {
            window.VFSPlugin.init();
        }, 800);
    };

    // Standard browser navigation (back/forward buttons)
    window.addEventListener("popstate", handleNavigation);

    // Mobile-specific events that can signify a page change
    window.addEventListener("pageshow", handleNavigation);
    window.addEventListener("focus", handleNavigation);

    // Monkey-patch history API to detect navigation
    const originalPushState = history.pushState;
    history.pushState = function () {
        originalPushState.apply(history, arguments);
        handleNavigation();
    };

    const originalReplaceState = history.replaceState;
    history.replaceState = function () {
        originalReplaceState.apply(history, arguments);
        handleNavigation();
    };

    // Handle tab visibility changes (e.g., user switches to another tab and back)
    document.addEventListener("visibilitychange", () => {
        if (!document.hidden) {
            console.debug('VFS: Page became visible, checking for tabs');
            setTimeout(() => window.customTabsPlugin.init(), 300);
        }
    });

    // Handle touch events which can also trigger navigation on mobile
    let touchNavigation = false;
    document.addEventListener("touchstart", () => {
        touchNavigation = true;
    });

    document.addEventListener("touchend", () => {
        if (touchNavigation) {
            setTimeout(() => window.customTabsPlugin.init(), 1000);
            touchNavigation = false;
        }
    });

    console.log('VFS: Plugin setup complete');
}
