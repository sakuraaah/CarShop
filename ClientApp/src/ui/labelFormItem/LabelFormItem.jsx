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

  const formattedAmount = (amount) => Intl.NumberFormat('en-US', {
    style: 'currency',
    currency: 'EUR'
  }).format(amount);

  return (
    <StyledLabelWrapper className={completeClassName}>
      <StyledLabel>
        {label + ':'}
      </StyledLabel>
      <StyledLabel className="bold">
        {' ' + (currency ? `${formattedAmount(labelValue)}` : (labelValue || '-'))}
      </StyledLabel>
    </StyledLabelWrapper>
  );
};
