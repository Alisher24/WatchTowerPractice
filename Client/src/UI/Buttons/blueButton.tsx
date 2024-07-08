import React from 'react';
import PropTypes from 'prop-types';

const BlueButton = (props) => {
  const { title } = props;
  return (
    <div>
      <button
        type="button"
        style={{
          background: '#1037AA',
          padding: '10px 16px',
          marginRight: '10px',
          border: '1px solid #E5E5E5',
          borderRadius: '8px',
          color: 'white',
          cursor: 'pointer',
        }}
      >
        {title}
      </button>
    </div>
  );
};
BlueButton.propTypes = {
  title: PropTypes.string.isRequired,
};
export default BlueButton;
