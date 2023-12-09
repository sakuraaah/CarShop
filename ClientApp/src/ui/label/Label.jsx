import React from 'react';
import styled from 'styled-components';

export const Label = (props) => {

  const StyledLabel = styled.label`
    font-size: 17px !important;

    &.extra-bold {
      font-weight: 600;
    }
  `;

  let completeClassName = '';

  if (props.className) {
    completeClassName += props.className;
  }

  if (props.extraBold) {
    completeClassName += ' extra-bold';
  }

  return (
    <StyledLabel className={`styled-label ${completeClassName}`} >
      {props.label}
    </StyledLabel>
  );
};
