import React from 'react';
import styled from 'styled-components';

export const Label = ({
  label,
  className,
  listItem,
  extraBold,
  currency = false
}) => {

  const StyledLabel = styled.label`
    font-size: 17px !important;

    &.list-item {
      font-size: 14px !important;
      margin-bottom: 8px;

      &.currency {
        font-size: 15px !important;
        color: black;
      }
    }

    &.extra-bold {
      font-weight: 600;
    }
  `;

  let completeClassName = '';

  if (className) {
    completeClassName += className;
  }

  if (listItem) {
    completeClassName += ' list-item';
  }

  if (currency) {
    completeClassName += ' currency';
  }

  if (extraBold) {
    completeClassName += ' extra-bold';
  }

  const formattedAmount = (amount) => Intl.NumberFormat('en-US', {
      style: 'currency',
      currency: 'EUR'
  }).format(amount);

  return (
    <StyledLabel className={`styled-label ${completeClassName}`} >
      {currency ? `${formattedAmount(label)}` : (label || '-')}
    </StyledLabel>
  );
};
