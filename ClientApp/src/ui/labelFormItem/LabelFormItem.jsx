import React from 'react';
import styled from 'styled-components';

export const LabelFormItem = (props) => {
  const StyledLabelWrapper = styled.div`
    display: flex;
  `;

  const StyledLabel = styled.label`
    &.bold {
      font-weight: 600;
    }

    &.pl {
      padding-left: 4px;
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
      <StyledLabel className="bold pl">
        {' ' + (props.labelValue || '-')}
      </StyledLabel>
    </StyledLabelWrapper>
  );
};
