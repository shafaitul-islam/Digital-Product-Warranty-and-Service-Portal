
/*
    DWS Global Theme Manager
    Controls Light and Dark themes across the entire application.
*/

(function () {

    "use strict";

    // Defines the local storage key used to save the theme preference.
    const THEME_KEY = "dws-theme";

    // Defines the default theme when no preference has been saved.
    const DEFAULT_THEME = "light";

    // Validates and normalizes the requested theme.
    function normalizeTheme(theme) {

        // Only dark and light themes are supported.
        return theme === "dark" ? "dark" : "light";
    }

    // Reads the previously saved theme preference.
    function getSavedTheme() {

        try {

            // Retrieves the saved theme from browser local storage.
            const savedTheme = localStorage.getItem(THEME_KEY);

            // Returns the saved theme if it is valid.
            if (savedTheme === "dark" || savedTheme === "light") {
                return savedTheme;
            }

            // Uses the default theme when no valid preference exists.
            return DEFAULT_THEME;

        } catch (error) {

            // Uses the default theme if browser storage is unavailable.
            console.warn("Unable to read the saved theme preference.", error);

            return DEFAULT_THEME;
        }
    }

    // Saves the selected theme in browser local storage.
    function saveTheme(theme) {

        try {

            // Stores the selected theme for future page visits.
            localStorage.setItem(THEME_KEY, theme);

        } catch (error) {

            // Theme switching continues even if saving is unavailable.
            console.warn("Unable to save the theme preference.", error);
        }
    }

    // Updates every theme toggle button available on the current page.
    function updateThemeButtons(theme) {

        // Finds all buttons marked as theme toggle controls.
        const buttons = document.querySelectorAll("[data-theme-toggle]");

        buttons.forEach(function (button) {

            // Finds the icon and label inside the current button.
            const icon = button.querySelector("[data-theme-icon]");
            const label = button.querySelector("[data-theme-label]");

            // Updates the button appearance for Dark Theme.
            if (theme === "dark") {

                if (icon) {
                    icon.textContent = "☀️";
                }

                if (label) {
                    label.textContent = "Light Mode";
                }

                button.setAttribute("aria-label", "Switch to light mode");

            } else {

                // Updates the button appearance for Light Theme.
                if (icon) {
                    icon.textContent = "🌙";
                }

                if (label) {
                    label.textContent = "Dark Mode";
                }

                button.setAttribute("aria-label", "Switch to dark mode");
            }

            // Updates the pressed state for accessibility.
            button.setAttribute(
                "aria-pressed",
                theme === "dark" ? "true" : "false"
            );
        });
    }

    // Applies the selected theme to the complete HTML document.
    function applyTheme(theme, shouldSave = true) {

        // Ensures that only supported theme values are applied.
        const selectedTheme = normalizeTheme(theme);

        // Updates the root HTML element for global CSS styling.
        document.documentElement.setAttribute(
            "data-theme",
            selectedTheme
        );

        // Saves the selected theme when requested.
        if (shouldSave) {
            saveTheme(selectedTheme);
        }

        // Updates all theme toggle buttons on the current page.
        updateThemeButtons(selectedTheme);
    }

    // Initializes the Global Theme Manager.
    function initializeTheme() {

        // Restores the customer's previously selected theme.
        applyTheme(getSavedTheme(), false);

        // Registers one global click event for all theme buttons.
        document.addEventListener("click", function (event) {

            // Ensures the event target is an HTML element.
            if (!(event.target instanceof Element)) {
                return;
            }

            // Finds the closest theme toggle button.
            const button = event.target.closest("[data-theme-toggle]");

            // Stops if the clicked element is not a theme button.
            if (!button) {
                return;
            }

            // Reads the currently active theme.
            const currentTheme =
                document.documentElement.getAttribute("data-theme");

            // Selects the opposite theme.
            const newTheme =
                currentTheme === "dark" ? "light" : "dark";

            // Applies the new theme and saves the preference.
            applyTheme(newTheme);
        });
    }

    // Applies the saved theme as early as possible.
    // This reduces the chance of displaying the wrong theme during page loading.
    applyTheme(getSavedTheme(), false);

    // Initializes theme buttons after the HTML document becomes ready.
    if (document.readyState === "loading") {

        document.addEventListener(
            "DOMContentLoaded",
            initializeTheme,
            { once: true }
        );

    } else {

        // Initializes immediately if the document is already loaded.
        initializeTheme();
    }

})();
