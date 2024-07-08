import {Global, css} from '@emotion/react';

export const BaseFont = 'font-size: 14px; font-style: normal;font-weight: normal;';
export const BaseFontFamily = '\'Poppins\', sans-serif';

export const BorderColor = '#e5e5e5';

const styles = css`
    * {
        vertical-align: top;
    }

    html {
        text-rendering: optimizeLegibility;
    }

    body {
        margin: 0;
        font-family: ${BaseFontFamily};
        ${BaseFont};
    }

    body a {
        text-decoration: none;
    }

    hr {
        border: 1px solid ${BorderColor};
    }
`;

const GlobalStyle = () => (
    <Global
        styles={styles}
    />
);

export default GlobalStyle;
