import React from 'react';

export const ConditionalWrapper = ({
  condition,
  wrapper: WrapperComponent,
  children
}) => {
  return condition ? <WrapperComponent>{children}</WrapperComponent> : <>{children}</>;
};
