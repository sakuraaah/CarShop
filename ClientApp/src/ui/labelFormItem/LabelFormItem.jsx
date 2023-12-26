import React from 'react';
import styled from 'styled-components';

export const LabelFormItem = ({
  className,
  column,
  label,
  labelValue,
  currency = false
}) => {
  const StyledLabelWrapper = styled.div`
    display: flex;
    flex-wrap: wrap;
    column-gap: 4px;

    &.column {
      flex-direction: column;
    }
  `;

  const StyledLabel = styled.label`
    &.bold {
      font-weight: 600;
    }
  `;

  let completeClassName = 'label-form-item ';

  if (className) {
    completeClassName += className;
  }

  if (column) {
    completeClassName += ' column';
  }

  return (
    <StyledLabelWrapper className={completeClassName}>
      <StyledLabel>
        {label + ':'}
      </StyledLabel>
      <StyledLabel className="bold">
        {' ' + (currency ? `${Number(labelValue).toFixed(2)}€` : (labelValue || '-'))}
      </StyledLabel>
    </StyledLabelWrapper>
  );
};
