import React from 'react';
import { Icon } from '../../ui';

export const Loader = ({
  loading,
  children
}) => {
  return loading ? <Icon faBase="far" icon="tractor" size="large" className="rotate btn-color auto-width" /> : <>{children}</>;
};
