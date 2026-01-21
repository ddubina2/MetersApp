/* eslint-disable react-hooks/exhaustive-deps */
import { Typography } from '@components/Typography';
import type { FC } from 'react';
import { useEffect, useId, useRef, useState, useTransition } from 'react';
import { twMerge } from 'tailwind-merge';
import type { TabsProps } from './types';
import React from 'react';

export const Tabs: FC<TabsProps> = ({
  items,
  onSelectTab,
  title,
  onKeyDown,
  currentTab,
  disabled,
  className,
  contentClassName,
  buttonClassName,
  textClassName,
  headerClassName,
  scrollCurrentTabIntoViewOptions,
}) => {
  const tabListId = useId();
  const tabRefs = useRef<(HTMLButtonElement | null)[]>([]);
  const [, startTransition] = useTransition();
  const preSelectedItem = items.findIndex(item => item.title === currentTab);
  const [openTab, setOpenTab] = useState(preSelectedItem !== -1 ? preSelectedItem : 0);

  useEffect(() => {
    setOpenTab(preSelectedItem !== -1 ? preSelectedItem : 0);
  }, [items]);

  const selectTab = (tabIndex: number) => {
    if (!disabled)
      startTransition(() => onSelectTab ? onSelectTab(items[tabIndex]) : setOpenTab(tabIndex));
  };

  useEffect(() => {
    selectTab(preSelectedItem !== -1 ? preSelectedItem : 0);
  }, [currentTab]);

  const handleKeyDown = (event: React.KeyboardEvent<HTMLButtonElement>) => {
    let keyHandled = false;
    const tabs = Array.from(document.querySelectorAll<HTMLButtonElement>(`[role="tab"][id^="tab-${tabListId}-"]`))
      .filter(link => !link.disabled);
    const currentIndex = tabs.findIndex(link => link.getAttribute('data-index') === event.currentTarget.getAttribute('data-index'));

    const focusTabAt = (index: number) => {
      const element = tabs[index];
      if (element)
        element.focus();
    };

    switch (event.key) {
      case 'ArrowLeft':
      case 'ArrowUp':
        focusTabAt((currentIndex - 1 + tabs.length) % tabs.length);
        keyHandled = true;
        break;

      case 'ArrowRight':
      case 'ArrowDown':
        focusTabAt((currentIndex + 1) % tabs.length);
        keyHandled = true;
        break;

      case 'Home':
        focusTabAt(0);
        keyHandled = true;
        break;

      case 'End':
        focusTabAt(tabs.length - 1);
        keyHandled = true;
        break;
    }

    if (keyHandled) {
      event.preventDefault();
      event.stopPropagation();
    }

    onKeyDown?.(event);
  };

  useEffect(() => {
    const options = scrollCurrentTabIntoViewOptions;
    if (options && tabRefs.current[openTab])
      tabRefs.current[openTab]?.scrollIntoView({ behavior: options.behavior, block: options.block, inline: options.inline });
  }, [openTab]);

  return (
    <section data-testid={`tabs-${tabListId}`} className={twMerge('flex h-full flex-col', className)}>
      <header
        role='tablist'
        aria-orientation='horizontal'
        aria-labelledby={title ? `tablist-${tabListId}` : undefined}
        className={twMerge('no-scrollbar flex w-full max-w-full shrink-0 overflow-auto rounded-t', headerClassName)}
      >
        {items.map((item, index) => (
          <button
            ref={(element) => { tabRefs.current[index] = element; }}
            onKeyDown={handleKeyDown}
            id={`tab-${tabListId}-${index}`}
            type='button'
            data-testid={`tab-${item.title}`}
            role='tab'
            aria-selected={openTab === index}
            aria-controls={`tabpanel-${index}-${tabListId}`}
            tabIndex={openTab === index ? undefined : -1}
            key={index}
            disabled={disabled || item.disabled}
            data-index={index}
            data-completed={item.completed ? 'true' : 'false'}
            className={twMerge(
              `inline-block shrink-0 cursor-pointer rounded-t px-5 focus:outline-none 
              focus-visible:bg-blue-100 disabled:cursor-not-allowed motion-safe:transition-colors 
              ${disabled ? '' : 'aria-selected:border-b-[2px]'}`,
              buttonClassName
            )}
            onClick={() => selectTab(index)}
          >
            {item.buttonContent ?? <Typography className={twMerge('py-1.5', textClassName)} text={item.title} />}
          </button>
        ))}
      </header>
      <main
        role='tabpanel'
        id='tab-content'
        className={twMerge('flex flex-col', contentClassName)}
      >
        {items[openTab]?.element}
      </main>
    </section>
  );
};
