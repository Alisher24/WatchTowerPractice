import React from 'react';
import PropTypes from 'prop-types';

const WhiteButton = (props) => {
  const { title } = props;
  return (
    <div>
      <button
        type="button"
        style={{
          background: 'white',
          padding: '10px 16px',
          marginRight: '10px',
          border: '1px solid #E5E5E5',
          borderRadius: '8px',
          cursor: 'pointer',
        }}
      >
        { title }
      </button>
    </div>
  );
};
WhiteButton.propTypes = {
  title: PropTypes.string.isRequired,
};
export default WhiteButton;
