import React from 'react';
import styled from 'styled-components';

export const LabelFormItem = (props) => {
  const StyledLabelWrapper = styled.div`
    display: flex;
    flex-wrap: wrap;
    column-gap: 4px;
  `;

  const StyledLabel = styled.label`
    &.bold {
      font-weight: 600;
    }
  `;

  let completeClassName = '';

  if (props.className) {
    completeClassName += props.className;
  }

  return (
    <StyledLabelWrapper className={completeClassName}>
      <StyledLabel>
        {props.label + ':'}
      </StyledLabel>
      <StyledLabel className="bold">
        {' ' + (props.labelValue || '-')}
      </StyledLabel>
    </StyledLabelWrapper>
  );
};
