import React from 'react';
import styled from 'styled-components';
import { ClassNameUtil } from '../../utils/className';

export const Icon = ({  // TODO
  className = '',
  icon = '',
  faBase = 'fas',
  color = '',
  size = '',
  roundBorder = false,
  baseClass = 'portal-icon',
  onClick,
}) => {
  const StyledIcon = styled.i`
    font-size: ${({ theme }) => theme.iconSize2};
    width: ${({ theme }) => theme.iconSize2};
    color: ${({ theme }) => theme.iconColor01};

    &.rotate {
      animation: rotation 2s infinite linear;
    }

    &.btn-color {
      color: ${({ theme }) => theme.brand03};
    }

    &.auto-width {
      width: auto;
      height: auto;
    }

    @keyframes rotation {
      from {
        transform: rotate(0deg);
      }
      to {
        transform: rotate(359deg);
      }
    }

    &.large:before {
      font-size: 40px;
    }

    &.green {
      color: forestgreen;
    }

    &.white {
      color: #ddd;
    }
  `;

  const iconClassName = new ClassNameUtil();
  iconClassName.add(baseClass);
  iconClassName.add(className);
  iconClassName.add(`${faBase} fa-${icon} `, !!icon);
  iconClassName.add(color);
  iconClassName.add(size);
  iconClassName.add(`${baseClass}__round-border`, roundBorder);

  return <StyledIcon className={iconClassName.getClassName()} onClick={onClick} />;
};
