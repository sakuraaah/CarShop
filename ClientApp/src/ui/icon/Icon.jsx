import React from 'react';
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
  const iconClassName = new ClassNameUtil();
  iconClassName.add(baseClass);
  iconClassName.add(className);
  iconClassName.add(`${faBase} fa-${icon} `, !!icon);
  iconClassName.add(color);
  iconClassName.add(size);
  iconClassName.add(`${baseClass}__round-border`, roundBorder);

  return <i className={iconClassName.getClassName()} onClick={onClick} />;
};
